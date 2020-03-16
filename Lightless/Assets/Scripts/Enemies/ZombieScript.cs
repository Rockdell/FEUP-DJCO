using System.Collections;
using UnityEngine;

public class ZombieScript : Entity
{
    // State
    private enum State { Init, Idle, Chase, Run, Die };
    private State currentState = State.Init;

    // Stats
    public float currentHealth;
    public float targetPosition { get; set; }

    private Animator animator;
    private AnimationClip deathAnimation;

    [Header("Zombie Weapons")]
    public WeaponData zombieBullet;
    private float currentZombieBulletCooldown = 0;
    private bool zombieBulletOnCooldown = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        deathAnimation = animator.runtimeAnimatorController.animationClips[1];
    }

    void Start()
    {
        StartCoroutine("Attack");
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
        currentHealth = 100.0f;
        SetState(State.Chase);
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightBullet"))     // Light bullet
        {
            currentHealth -= collision.gameObject.GetComponent<LightBulletScript>().weaponData.weaponDamage;
        }
        else if (collision.gameObject.CompareTag("Grenade"))    // Grenade
        {
            currentHealth -= collision.gameObject.GetComponent<GrenadeScript>().weaponData.weaponDamage;
        }

        // Check health
        if (currentHealth <= 0)
        {
            SetState(State.Die);

            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(deathAnimation.length);
            gameObject.SetActive(false);
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
                Behaviour = new ScrollableBehaviour(10.0f);
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            case State.Run:
                Behaviour = new ScrollableBehaviour(0.0f);
                GetComponent<SpriteRenderer>().flipX = true;
                break;
            case State.Die:
                Behaviour = new ScrollableBehaviour(20.0f);
                break;
        }

        currentState = nextState;
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (currentState != State.Die)
            {
                if (currentState == State.Run)  
                {
                    animator.SetTrigger("jump");
                }
                else
                {
                    var targetPosition = GameManager.Instance.GetPlayer().transform.position;

                    if (!zombieBulletOnCooldown)
                    {
                        if (Vector2.Distance(targetPosition, EntityBody.position) < zombieBullet.weaponRange)
                        {
                            zombieBulletOnCooldown = true;
                            GameObject zombieBullet = GameManager.Instance.GetObject(GameManager.ObjectType.ZombieBullet);
                            zombieBullet.GetComponent<ZombieBulletScript>().Spawn(EntityBody.position, Quaternion.identity);
                            zombieBullet.SetActive(true);
                            zombieBullet.GetComponent<ZombieBulletScript>().Shoot(targetPosition);

                        }
                    }
                }
            }

            yield return new WaitForSeconds(Random.Range(0, 2));
        }
    }
}

public class ZombieSM
{
    public enum State
    {
        RunTo, RunFrom, Attack, Jump, Die
    };

    private State currentState;

    public ZombieSM()
    {
        currentState = State.RunTo;
    }

    void SetState(State nextState)
    {
        currentState = nextState;
    }
}
