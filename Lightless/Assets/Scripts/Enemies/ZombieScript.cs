using System.Collections;
using UnityEngine;

public class ZombieScript : Entity
{
    [Header("Zombie Stats")]
    public EnemyData enemyData;
    private float currentHealth;
    public float targetPosition { get; set; }

    [Header("Zombie Weapons")]
    public WeaponData zombieBullet;
    private float currentZombieBulletCooldown = 0;
    private bool zombieBulletOnCooldown = false;

    // State
    private enum State { Init, Idle, Chase, Run, Die };
    private State currentState = State.Init;

    // Animations
    private Animator animator;
    private AnimationClip deathAnimation;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        deathAnimation = animator.runtimeAnimatorController.animationClips[1];
    }

    void Start()
    {
        StartCoroutine(Attack());
    }

    void Update()
    {
        if (currentState == State.Die)
            return;

        if (currentState == State.Idle && currentHealth <= 30)
        {
            SetState(State.Run);
        }
        else
        {
            if (EntityBody.position.x - targetPosition <= Mathf.Epsilon)
            {
                SetState(State.Idle);
            }
        }

        //Cooldowns
        if (zombieBulletOnCooldown)
        {
            if (currentZombieBulletCooldown < zombieBullet.weaponCooldown)
            {
                currentZombieBulletCooldown += Time.deltaTime;
            }
            else
            {
                zombieBulletOnCooldown = false;
                currentZombieBulletCooldown = 0;
            }
        }
    }

    void OnEnable()
    {
        currentHealth = enemyData.maxHealth;
        SetState(State.Chase);
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, enemyData.maxHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    void SetState(State nextState)
    {
        if (currentState == nextState)
            return;

        switch (nextState)
        {
            case State.Idle:
                Behaviour = new ScrollableBehaviour(0.0f);
                break;
            case State.Chase:
                Behaviour = new ScrollableBehaviour(enemyData.moveSpeed);
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            case State.Run:
                Behaviour = new ScrollableBehaviour(0.0f);
                GetComponent<SpriteRenderer>().flipX = true;
                break;
            case State.Die:
                Behaviour = new ScrollableBehaviour(2 * enemyData.moveSpeed);
                break;
        }

        currentState = nextState;
    }

    IEnumerator Attack()
    {
        while (true)
        {

            if (currentState == State.Run)  
            {
                animator.SetTrigger("jump");
            }
            else if (currentState == State.Idle || currentState == State.Chase)
            {
                var targetPosition = GameManager.Instance.GetPlayer().transform.position;

                if (!zombieBulletOnCooldown)
                {
                    if (Vector2.Distance(targetPosition, EntityBody.position) <= zombieBullet.weaponRange)
                    {
                        zombieBulletOnCooldown = true;
                        GameObject zombieBullet = GameManager.Instance.GetObject(GameManager.ObjectType.ZombieBullet);
                        zombieBullet.GetComponent<ZombieBulletScript>().Spawn(EntityBody.position, Quaternion.identity);
                        zombieBullet.SetActive(true);
                        zombieBullet.GetComponent<ZombieBulletScript>().Shoot(targetPosition);

                    }
                }
            }

            yield return new WaitForSeconds(Random.Range(0, 2));
        }
    }

    IEnumerator Die()
    {
        SetState(State.Die);
        currentHealth = 0;

        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(deathAnimation.length);
        gameObject.SetActive(false);
    }
}
