using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerScript : MonoBehaviour {

    [Header("Player Stats")]
    public float moveSpeed;
    public float maxHealth;
    public float healthLossPerSecond;
    public float maxLightRadius;
    public float minLightRadius;
    private float currentHealth;

    [Header("Player Weapons")]
    public WeaponData lightBullet;
    private float currentLightBulletCooldown = 0;
    private bool lightBulletOnCooldown = false;

    public WeaponData grenade;
    private float currentGrenadeCooldown = 0;
    private bool grenadeOnCooldown = false;

    [Header("Player Options")]
    public HealthBarScript healthBarUI;
    public CooldownScript grenadeCooldownUI;
    public GameObject crosshair;
    public int flashCount;
    public float flashDuration;
    private Rigidbody2D rb;
    private Light2D light2D;
    private float currentFlashDuration = 0;
    private int currentFlashCount = 0;
    
    public float collisionDamage;
    public float collisionDamangeCooldown;
    private float currentCollisionDamageCooldown = 0;
    private bool collisionDamageOnCooldown = false;

    /* Inputs */
    private Vector2 movementInput;
    private Vector2 crosshairInput;
    private bool isShooting = false;
    private bool powerUpActivated = false;

    private SpriteRenderer sprite;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        light2D = GetComponent<Light2D>();
        light2D.pointLightOuterRadius = maxLightRadius;
        sprite = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        grenadeCooldownUI.SetMaxCooldown(grenade.weaponCooldown);
    }

    void Update() 
    {
        if (GameManager.Instance.isGameOver)
            return;

        crosshair.transform.position = new Vector3(crosshairInput.x, crosshairInput.y, -1);
        light2D.pointLightOuterRadius = minLightRadius + (maxLightRadius - minLightRadius) * (currentHealth / maxHealth);

        //Update HP (Light)
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthBarUI.SetHealth((int)currentHealth);

            GameManager.Instance.GameOver();
        }
        else 
        {
            currentHealth -= healthLossPerSecond * Time.deltaTime;
            healthBarUI.SetHealth((int)currentHealth);
        }

        //Cooldowns
        if (lightBulletOnCooldown) 
        {
            if (currentLightBulletCooldown < lightBullet.weaponCooldown)
            {
                currentLightBulletCooldown += Time.deltaTime;
            }
            else 
            {
                lightBulletOnCooldown = false;
                currentLightBulletCooldown = 0;
            }
        }

        if (grenadeOnCooldown) 
        {
            if (currentGrenadeCooldown < grenade.weaponCooldown) 
            {
                currentGrenadeCooldown += Time.deltaTime;
                grenadeCooldownUI.SetCooldown(currentGrenadeCooldown);
            }
            else 
            {
                grenadeOnCooldown = false;
                currentGrenadeCooldown = 0;
                grenadeCooldownUI.SetCooldown(grenade.weaponCooldown);
            }
        }

        // Flash
        if (currentFlashCount > 0) 
        {
            AudioManager.Instance.Play("PlayerTakingDamage");
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

        // Collision damage
        if (collisionDamageOnCooldown)
        {
            if (currentCollisionDamageCooldown < collisionDamangeCooldown)
            {
                currentCollisionDamageCooldown += Time.deltaTime;
            }
            else
            {
                collisionDamageOnCooldown = false;
                currentCollisionDamageCooldown = 0;
            }
        }
    }

    void FixedUpdate() 
    {
        //Update Rigibody position
        Vector2 nextPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -GameManager.Instance.screenBounds.x, GameManager.Instance.screenBounds.x), Mathf.Clamp(nextPosition.y, -GameManager.Instance.screenBounds.y, GameManager.Instance.screenBounds.y)));

        if (isShooting)
            Shoot();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collisionDamageOnCooldown && (collision.gameObject.CompareTag("Zombie") || collision.gameObject.CompareTag("Firefly") || collision.gameObject.CompareTag("Boss")))
        {
            collisionDamageOnCooldown = true;
            ChangeHealth(-collisionDamage);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collisionDamageOnCooldown && (collision.gameObject.CompareTag("Zombie") || collision.gameObject.CompareTag("Firefly") || collision.gameObject.CompareTag("Boss")))
        {
            collisionDamageOnCooldown = true;
            ChangeHealth(-collisionDamage);
        }
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (value < 0)
            currentFlashCount = flashCount;
    }

    public void Shoot() 
    {
        if (!lightBulletOnCooldown && !powerUpActivated) 
        {
            lightBulletOnCooldown = true;
            GameObject lightBullet = GameManager.Instance.GetObject(GameManager.ObjectType.LightBullet);
            lightBullet.SetActive(true);
            lightBullet.GetComponent<LightBulletScript>().Shoot(crosshairInput);
        }
    }

    public void ThrowGrenade()
    {
        if (!grenadeOnCooldown) 
        {
            grenadeOnCooldown = true;
            GameObject grenade = GameManager.Instance.GetObject(GameManager.ObjectType.Grenade);
            grenade.SetActive(true);
            grenade.GetComponent<GrenadeScript>().Throw(crosshairInput);
        }
    }

    public Vector2 GetCrosshairInput()
    {
        return crosshairInput;
    }

    public bool GetPowerUpInput() 
    {
        return powerUpActivated;
    }

    public void UpdateMovement(Vector2 input) 
    {
        movementInput = input;
    }

    public void UpdateCrosshair(Vector2 input) 
    {
        crosshairInput = GameManager.Instance.gameCamera.ScreenToWorldPoint(new Vector3(input.x, input.y));
    }

    public void UpdatePowerUpInput() 
    {
        powerUpActivated = !powerUpActivated;
    }
    
    public void UpdateShootInput() 
    {
        isShooting = !isShooting;
    }

    public bool isDead() {
        return currentHealth <= 0;
    }

}
