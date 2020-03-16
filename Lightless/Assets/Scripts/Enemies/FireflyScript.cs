using System.Collections;
using UnityEngine;

public class FireflyScript : Entity
{
    [Header("Firefly Stats")]
    public EnemyData enemyData;
    private float currentHealth;
    public float sleep { get; set; }

    [Header("Firefly Weapons")]
    public WeaponData fireflyExplosion;

    // State
    private enum State { Init, Idle, Chase, Die };
    private State currentState = State.Init;
    private bool playerHit;

    // Animations
    private Animator animator;
    private AnimationClip deathAnimation;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        deathAnimation = animator.runtimeAnimatorController.animationClips[1];
    }

    void Update()
    {
        if (currentState == State.Die)
            return;
       
        if (currentState == State.Idle)
        {
            sleep -= Time.deltaTime;

            if (sleep <= 0)
            {
                SetState(State.Chase);
            }

            return;
        }
        
        var targetPosition = GameManager.Instance.GetPlayer().transform.position;

        if (Vector2.Distance(targetPosition, EntityBody.position) <= fireflyExplosion.weaponRange)
        {
            StartCoroutine(Die());
        }
    }

    void OnEnable()
    {
        currentHealth = enemyData.maxHealth;
        SetState(State.Idle);
        playerHit = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check animation for trigger radius only on explosion

        if (!playerHit && collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerScript>().ChangeHealth(-fireflyExplosion.weaponDamage);
            playerHit = true;
        }
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
                Behaviour = new HomingBehaviour(enemyData.moveSpeed);
                break;
            case State.Die:
                Behaviour = new HomingBehaviour(enemyData.moveSpeed / 3);
                break;
        }

        currentState = nextState;
    }

    IEnumerator Die()
    {
        SetState(State.Die);
        currentHealth = 0;

        //EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(deathAnimation.length);
        //EntityBody.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }
}
