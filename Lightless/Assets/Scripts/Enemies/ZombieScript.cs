using System.Collections;
using UnityEngine;

public class ZombieScript : Entity
{
    private float currentHealth;
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
        // TODO Improve later
        if (EntityBody.position.x - targetPosition <= Mathf.Epsilon)
        {
            var runningFromPlayer = new ScrollableBehaviour(0f);
            Behaviour = runningFromPlayer;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            var runningToPlayer = new ScrollableBehaviour();
            Behaviour = runningToPlayer;
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
        Behaviour = new ScrollableBehaviour();
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
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(deathAnimation.length);
            gameObject.SetActive(false);
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            var targetPosition = GameManager.Instance.GetPlayer().transform.position;

            if (!zombieBulletOnCooldown)
            {
                //if (Vector2.Distance(targetPosition, EntityBody.position) < zombieBullet.weaponRange)
                //{
                //}

                zombieBulletOnCooldown = true;
                GameObject zombieBullet = GameManager.Instance.GetObject(GameManager.ObjectType.ZombieBullet);
                zombieBullet.GetComponent<ZombieBulletScript>().Spawn(EntityBody.position, Quaternion.identity);
                zombieBullet.SetActive(true);
                zombieBullet.GetComponent<ZombieBulletScript>().Shoot(targetPosition);
            }

            yield return new WaitForSeconds(Random.Range(1, 2));
        }
    }
}
