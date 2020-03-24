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

    [Header("Boss Weapons")]
    public WeaponData darkBullet;

    private float fireflyCooldown = 1.8f;
    private float currentFireflyCooldown = 0.0f;
    private bool fireflyOnCooldown = false;

    private float roamCooldown = 5.0f;
    private float currentRoamCooldown = 0.0f;
    private bool roamOnCooldown = false;

    private Light2D light2D;

    // State
    private enum State { Init, Idle, Chase, Roam, Die };
    private State currentState;

    // Animations
    private Animator animator;
    private AnimationClip deathAnimation;

    // Flash
    private SpriteRenderer sprite;
    private int flashCount = 5;
    private float flashDuration = 0.1f;
    private float currentFlashDuration = 0.0f;
    private int currentFlashCount = 0;

    private GameObject healthBarUI;
    private BossHealthBarScript healthBar;
    private Animator healthBarAnimator;

    protected override void Awake()
    {
        base.Awake();
        light2D = GetComponent<Light2D>();
        animator = GetComponent<Animator>();
        deathAnimation = animator.runtimeAnimatorController.animationClips[1];
        sprite = GetComponent<SpriteRenderer>();

        healthBarUI = GameObject.FindGameObjectWithTag("BossHealthBar");
        healthBar = healthBarUI.GetComponent<BossHealthBarScript>();
        healthBarAnimator = healthBarUI.GetComponent<Animator>();
    }


    void Update()
    {
        if (currentState == State.Die)
            return;

        if (currentState == State.Chase && EntityBody.position.x < GameManager.Instance.screenBounds.x / 2)
        {
            roamOnCooldown = true;

            SetState(State.Idle);
        }
        else if (currentState == State.Idle && !roamOnCooldown)
        {
            roamOnCooldown = true;

            SetState(State.Roam);
        }
        else if (currentState == State.Roam && !roamOnCooldown)
        {
            roamOnCooldown = true;

            SetState(State.Idle);
        }

        // Cooldowns
        if (fireflyOnCooldown)
        {
            if (currentFireflyCooldown < fireflyCooldown)
            {
                currentFireflyCooldown += Time.deltaTime;
            }
            else
            {
                fireflyOnCooldown = false;
                currentFireflyCooldown = 0.0f;
            }
        }

        if (roamOnCooldown)
        {
            if (currentRoamCooldown < roamCooldown)
            {
                currentRoamCooldown += Time.deltaTime;
            }
            else
            {
                roamOnCooldown = false;
                currentRoamCooldown = 0.0f;
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

        light2D.pointLightOuterRadius = minLightRadius + (maxLightRadius - minLightRadius) * (currentHealth / maxHealth);
    }

    void OnEnable()
    {
        healthBarUI.SetActive(true);

        light2D.pointLightOuterRadius = maxLightRadius;

        currentHealth = maxHealth;
        currentState = State.Init;

        healthBarAnimator.SetBool("showUp", true);
        healthBar.SetMaxHealth(maxHealth);

        SetState(State.Chase);
        StartCoroutine(Act());
    }

    void OnDisable()
    {
        if (healthBarUI != null)
            healthBarUI.SetActive(false);
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

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
                Behaviour = new FlowPointBehaviour(new Vector2(GameManager.Instance.screenBounds.x / 2.0f, 0.0f), moveSpeed);
                break;
            case State.Chase:
                Behaviour = new ScrollableBehaviour(moveSpeed);
                break;
            case State.Roam:
                Behaviour = new RoamingBehaviour(moveSpeed);
                break;
            case State.Die:
                Behaviour = new ScrollableBehaviour(0.0f);
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
            if (currentState == State.Idle)
            {
                if (!fireflyOnCooldown)
                {
                    fireflyOnCooldown = true;
                    GameObject firefly = GameManager.Instance.GetObject(GameManager.ObjectType.Firefly);
                    firefly.GetComponent<FireflyScript>().Spawn(GameObject.FindGameObjectWithTag("BossProjectileStart").transform.position, Quaternion.identity);
                    firefly.SetActive(true);
                }

                yield return new WaitForSeconds(0.5f);
            }
            else if (currentState == State.Roam)
            {
                float offset = 3.0f;
                float angle = 0.0f, incAngle = 30.0f;
                Vector2 projectileDirection = Vector2.right;

                while (angle < 360)
                {
                    GameObject darkBullet = GameManager.Instance.GetObject(GameManager.ObjectType.DarkBullet);
                    darkBullet.GetComponent<DarkBulletScript>().Spawn(EntityBody.position + offset * projectileDirection, Quaternion.identity);
                    darkBullet.SetActive(true);
                    darkBullet.GetComponent<DarkBulletScript>().Shoot(EntityBody.position + (offset + 1.0f) * projectileDirection);

                    angle += incAngle;
                    projectileDirection = Quaternion.Euler(0.0f, 0.0f, incAngle) * projectileDirection;
                }

                yield return new WaitForSeconds(0.8f);
            }
            else if (currentState == State.Die)
            {
                animator.SetBool("isDead", true);
                yield return new WaitForSeconds(deathAnimation.length);
                gameObject.SetActive(false);
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
