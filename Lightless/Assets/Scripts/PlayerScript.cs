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

    /* Inputs */
    private Vector2 movementInput;
    private Vector2 crosshairInput;
    private bool isShooting = false;
    private bool powerUpActivated = false;

    //private bool isAiming = false;
    private SpriteRenderer sprite;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        light2D = GetComponent<Light2D>();
        light2D.pointLightOuterRadius = maxLightRadius;
        sprite = GetComponent<SpriteRenderer>();

        //Cursor.visible = false;

        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        grenadeCooldownUI.SetMaxCooldown(grenade.weaponCooldown);
    }

    // Update is called once per frame
    void Update() 
    {
        crosshair.transform.position = new Vector2(crosshairInput.x, crosshairInput.y);
        light2D.pointLightOuterRadius = minLightRadius + (maxLightRadius - minLightRadius) * (currentHealth / maxHealth);

        //Update HP (Light)
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthBarUI.SetHealth((int)currentHealth);
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
    }

    void FixedUpdate() 
    {
        //Update Rigibody position
        Vector2 nextPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -GameManager.Instance.screenBounds.x, GameManager.Instance.screenBounds.x), Mathf.Clamp(nextPosition.y, -GameManager.Instance.screenBounds.y, GameManager.Instance.screenBounds.y)));

        if (isShooting)
            Shoot();
    }

    public void ChangeHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (value < 0)
            currentFlashCount = flashCount;
    }

    public void Shoot() 
    {
        if (!lightBulletOnCooldown) 
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
        crosshairInput = GameManager.Instance.gameCamera.ScreenToWorldPoint(new Vector3(input.x, input.y, 0));
    }

    public void UpdatePowerUpInput() 
    {
        powerUpActivated = !powerUpActivated;
    }
    
    public void UpdateShootInput() 
    {
        isShooting = !isShooting;
    }

}
