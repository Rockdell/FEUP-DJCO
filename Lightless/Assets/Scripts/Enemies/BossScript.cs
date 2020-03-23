using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BossScript : Entity
{
    [Header("Boss Stats")]
    public float moveSpeed;
    public float maxHealth;
    public float maxLightRadius;
    public float minLightRadius;
    private float currentHealth;

    private Light2D light2D;

    // State
    private enum State { Init, Idle, Chase, Die };
    private State currentState;

    protected override void Awake()
    {
        base.Awake();
        light2D = GetComponent<Light2D>();
    }

    private void Start()
    {
        light2D.pointLightOuterRadius = maxLightRadius;

        currentHealth = maxHealth;
        currentState = State.Init;

        SetState(State.Chase);
        StartCoroutine(Act());
    }

    void Update()
    {
        light2D.pointLightOuterRadius = minLightRadius + (maxLightRadius - minLightRadius) * (currentHealth / maxHealth);

        if (currentState == State.Die)
            return;

        if (currentState == State.Chase && EntityBody.position.x < GameManager.Instance.screenBounds.x / 2)
        {
            SetState(State.Idle);
        }
    }

    //void OnEnable()
    //{
    //    light2D.pointLightOuterRadius = maxLightRadius;

    //    currentHealth = maxHealth;
    //    currentState = State.Init;

    //    SetState(State.Chase);
    //    StartCoroutine(Act());
    //}

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
    }

    void SetState(State nextState)
    {
        if (currentState == nextState)
            return;

        switch (nextState)
        {
            case State.Idle:
                Behaviour = new RoamingBehaviour(moveSpeed);
                break;
            case State.Chase:
                Behaviour = new ScrollableBehaviour(moveSpeed);
                break;
            //case State.Die:
            //    Behaviour = new ScrollableBehaviour(2 * enemyData.moveSpeed);
            //    currentHealth = 0;
            //    break;
        }

        currentState = nextState;
    }

    IEnumerator Act()
    {
        yield return new WaitForFixedUpdate();

        while (true)
        {
            if (currentState != State.Idle)
            {
                yield return new WaitForSeconds(1.0f);
                continue;
            }

            // Do stuff

            yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        }
    }
}
