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

    void OnEnable()
    {
        currentHealth = enemyData.maxHealth;
        SetState(State.Idle);
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, enemyData.maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void SetState(State nextState)
    {
        if (currentState == nextState)
            return;

        switch (nextState)
        {
            case State.Idle:
                Behaviour = new ScrollableBehaviour(10.0f);
                break;
            case State.Chase:
                Behaviour = new ScrollableBehaviour(10.0f);
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            case State.Die:
                Behaviour = new ScrollableBehaviour(0.0f);
                break;
        }

        currentState = nextState;
    }

    IEnumerator Die()
    {
        SetState(State.Die);

        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(deathAnimation.length);
        gameObject.SetActive(false);
    }
}
