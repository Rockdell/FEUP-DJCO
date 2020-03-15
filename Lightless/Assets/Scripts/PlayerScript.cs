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
    //public GameObject grenadeStart;
    private Rigidbody2D rb;
    private Light2D light2D;

    /* Trajectory */
    [Header("Trajectory Settings")]
    public LineRenderer lineVisual;
    public int linePrecision;

    /* Inputs */
    private Vector2 movementInput;
    private Vector2 crosshairInput;

    //private bool isAiming = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        light2D = GetComponent<Light2D>();
        light2D.pointLightOuterRadius = maxLightRadius;

        //Cursor.visible = false;

        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        grenadeCooldownUI.SetMaxCooldown(grenade.weaponCooldown);
    }

    // Update is called once per frame
    void Update() {
        crosshair.transform.position = new Vector2(crosshairInput.x, crosshairInput.y);
        light2D.pointLightOuterRadius = minLightRadius + (maxLightRadius - minLightRadius) * (currentHealth / maxHealth);

        //if (isAiming)
        //    DrawTrajectory();

        //Update HP (Light)
        if (currentHealth <= 0)
            Debug.Log("You Died");
        else {
            currentHealth -= healthLossPerSecond * Time.deltaTime;
            healthBarUI.SetHealth((int)currentHealth);
        }

        //Cooldowns
        if (lightBulletOnCooldown) {
            if (currentLightBulletCooldown < lightBullet.weaponCooldown) {
                currentLightBulletCooldown += Time.deltaTime;
            }
            else {
                lightBulletOnCooldown = false;
                currentLightBulletCooldown = 0;
            }
        }

        if (grenadeOnCooldown) {
            if (currentGrenadeCooldown < grenade.weaponCooldown) {
                currentGrenadeCooldown += Time.deltaTime;
                grenadeCooldownUI.SetCooldown(currentGrenadeCooldown);
            }
            else {
                grenadeOnCooldown = false;
                currentGrenadeCooldown = 0;
                grenadeCooldownUI.SetCooldown(grenade.weaponCooldown);
            }
        }
    }

    void FixedUpdate() {
        //Update Rigibody position
        Vector2 nextPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -GameManager.Instance.screenBounds.x, GameManager.Instance.screenBounds.x), Mathf.Clamp(nextPosition.y, -GameManager.Instance.screenBounds.y, GameManager.Instance.screenBounds.y)));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DropLight"))   // Drop light
        {
            currentHealth = Mathf.Min(currentHealth + 20, maxHealth);
            healthBarUI.SetHealth((int)currentHealth);
        }
        else if (collision.gameObject.CompareTag("ZombieBullet"))   // Zombie bullet
        {
            currentHealth -= collision.gameObject.GetComponent<ZombieBulletScript>().weaponData.weaponDamage;
            healthBarUI.SetHealth((int)currentHealth);
        }
    }

    public void Shoot() {
        if (!lightBulletOnCooldown) {
            lightBulletOnCooldown = true;
            GameObject lightBullet = GameManager.Instance.GetObject(GameManager.ObjectType.LightBullet);
            lightBullet.SetActive(true);
            lightBullet.GetComponent<LightBulletScript>().Shoot(crosshairInput);
        }
    }

    public void ThrowGrenade() {
        //isAiming = false;
        //lineVisual.positionCount = 0;

        if (!grenadeOnCooldown) {
            grenadeOnCooldown = true;
            GameObject grenade = GameManager.Instance.GetObject(GameManager.ObjectType.Grenade);
            grenade.SetActive(true);
            grenade.GetComponent<GrenadeScript>().Throw(crosshairInput);
        }
    }

    private void DrawTrajectory() {
        //float timePerSegment = (crosshairInput.x - grenadeStart.transform.position.x) / (horizontalVelocity * linePrecision);
        //Vector3 initialPosition = grenadeStart.transform.position;
        //for (int i = 0; i < linePrecision; i++) {
        //    lineVisual.SetPosition(i, Tools.CalculatePositionInTime(initialPosition, Tools.CalculateVerticalVelocity(initialPosition, crosshairInput, horizontalVelocity), i * timePerSegment));
        //}
    }

    public void UpdateMovement(Vector2 input) {
        movementInput = input;
    }

    public void UpdateCrosshair(Vector2 input) {
        crosshairInput = GameManager.Instance.gameCamera.ScreenToWorldPoint(new Vector3(input.x, input.y, 0));
    }

    public void SetIsAimingToTrue()
    {
        //isAiming = true;
    }

}
