using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyScript : Entity
{
    [Header("Firefly Stats")]
    public EnemyData enemyData;
    private float currentHealth;

    [Header("Firefly Weapons")]
    public WeaponData fireflyExplosion;

    // State
    private enum State { Init, Idle, Chase, Die };
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
        //StartCoroutine("Attack");
    }

    void Update()
    {
        if (currentState == State.Die)
            return;
    }

    void OnEnable()
    {
        currentHealth = enemyData.maxHealth;
        SetState(State.Idle);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hit");
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, enemyData.maxHealth);

        Debug.Log(currentHealth);

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
                Behaviour = new ScrollableBehaviour(0);
                break;
            case State.Chase:
                Behaviour = new ScrollableBehaviour(0);
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            case State.Die:
                Behaviour = new ScrollableBehaviour(0.0f);
                break;
        }

        currentState = nextState;
    }

    //IEnumerator Attack()
    //{

    //}

    IEnumerator Die()
    {
        SetState(State.Die);

        EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(deathAnimation.length);
        EntityBody.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }
}
