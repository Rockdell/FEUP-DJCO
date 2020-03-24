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
    private float currentZombieBulletCooldown;
    private bool zombieBulletOnCooldown;

    // State
    private enum State { Init, Idle, Chase, Run, Die };
    private State currentState;

    // Animations
    private Animator animator;
    private AnimationClip jumpAnimation;
    private AnimationClip deathAnimation;

    // Flash
    private SpriteRenderer sprite;
    private int flashCount = 5;
    private float flashDuration = 0.1f;
    private float currentFlashDuration = 0.0f;
    private int currentFlashCount = 0;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        jumpAnimation = animator.runtimeAnimatorController.animationClips[1];
        deathAnimation = animator.runtimeAnimatorController.animationClips[2];
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (currentState == State.Die)
            return;

        if (currentState == State.Idle && currentHealth <= 30)
        {
            SetState(State.Run);
        }
        else if (currentState != State.Run)
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

        // Flash
        if (currentFlashCount > 0)
        {
            if (currentFlashDuration > 0)
            {
                currentFlashDuration -= Time.deltaTime;
            }
            else
            {
                currentFlashDuration = flashDuration;
                sprite.enabled = !sprite.enabled;
                currentFlashCount--;
            }
        }
        else
        {
            currentFlashDuration = 0;
            sprite.enabled = true;
        }
    }

    void OnEnable()
    {
        currentZombieBulletCooldown = 0;
        zombieBulletOnCooldown = false;
        
        currentState = State.Init;
        currentHealth = enemyData.maxHealth;

        SetState(State.Chase);
        StartCoroutine(Act());
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, enemyData.maxHealth);

        currentFlashCount = flashCount;

        if (currentHealth <= 0)
        {
            SetState(State.Die);
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
                currentHealth = 0;
                break;
        }

        currentState = nextState;
    }

    IEnumerator Act()
    {
        yield return new WaitForFixedUpdate();

        while (true)
        {
            if (currentState == State.Idle || currentState == State.Chase)
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

                yield return new WaitForSeconds(0.5f);
            }
            else if (currentState == State.Run)
            {
                animator.SetTrigger("jump");
                yield return new WaitForSeconds(jumpAnimation.length);
            }
            else if (currentState == State.Die)
            {
                animator.SetBool("isDead", true);
                yield return new WaitForSeconds(deathAnimation.length);
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
