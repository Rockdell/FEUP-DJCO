using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RedLightScript : Entity
{
    [Header("Red Light Stats")]
    public EnemyData enemyData;
    public float shakeDuration = 0.3f;
    private float currentHealth;
    private float currentShakeDuration = 0f;
    private float initialRotation;

    [Header("Red Light Weapons")]
    public WeaponData lightRay;
    private float currentWeaponCooldown = 0f;

    [Header("Raycast Options")]
    public int anglePerRaycast = 3;

    private Light2D ambientLight;
    private Light2D redLight;
    private float innerAngle, outerAngle;

    protected override void Awake()
    {
        base.Awake();
        initialRotation = EntityBody.rotation;
        ambientLight = transform.GetChild(0).GetComponent<Light2D>();
        redLight = transform.GetChild(1).GetComponent<Light2D>();

        (innerAngle, outerAngle) = (redLight.pointLightInnerAngle, redLight.pointLightOuterAngle);
    }

    void Start()
    {
        SetBehaviour(new ScrollableBehaviour(enemyData.moveSpeed));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentShakeDuration > 0)
        {
            EntityBody.rotation = initialRotation + Random.Range(-4f, 4f);
            currentShakeDuration -= Time.fixedDeltaTime;
        }
        else
        {
            currentShakeDuration = 0;
            EntityBody.rotation = initialRotation;
        }

        if (currentWeaponCooldown <= 0)
        {
            currentWeaponCooldown = 0;
            for (int i = 0; i <= redLight.pointLightOuterAngle / anglePerRaycast && currentHealth > 0; i++)
            {
                Vector2 direction = Quaternion.AngleAxis(-redLight.pointLightOuterAngle / 2f + anglePerRaycast * i, Vector3.forward) * Vector2.down;

                // Cast a ray straight down.
                RaycastHit2D hit = Physics2D.Raycast(redLight.transform.position, direction, 50, 1 << LayerMask.NameToLayer("Player"));

                // If it hits the player
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<PlayerScript>().ChangeHealth(-lightRay.weaponDamage);
                    currentWeaponCooldown = lightRay.weaponCooldown;
                }
            }
        }
        else
        {
            currentWeaponCooldown -= Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Boundary") && collider.gameObject.name == "Boundary Left")
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        currentHealth = enemyData.maxHealth;
        ambientLight.gameObject.SetActive(true);
        redLight.gameObject.SetActive(true);

        redLight.pointLightInnerAngle = innerAngle;
        redLight.pointLightOuterAngle = outerAngle;
        AudioManager.Instance.Play("LightbulbSound");
    }

    private void OnDisable()
    {
        AudioManager.Instance.Stop("LightbulbSound");
    }

    public void ChangeHealth(bool isGrenade = false)
    {
        if (currentHealth <= 0)
            return;

        currentShakeDuration = shakeDuration;

        if (isGrenade)
            currentHealth = 0f;
        else
            currentHealth = Mathf.Clamp(currentHealth - 0.3f, 0, enemyData.maxHealth);

        if (currentHealth <= 0.1f)
        {
            currentHealth = 0;
        }

        redLight.pointLightInnerAngle *= currentHealth;
        redLight.pointLightOuterAngle *= currentHealth;

        if (currentHealth == 0)
        {
            ambientLight.gameObject.SetActive(false);
            redLight.gameObject.SetActive(false);
            AudioManager.Instance.Stop("LightbulbSound");
        }

        AudioManager.Instance.Play("LightbulbHit");
    }
}
