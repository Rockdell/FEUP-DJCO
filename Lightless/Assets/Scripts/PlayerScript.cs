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
    public float lightBulletCooldown;
    private float currentLightBulletCooldown = 0;
    private bool lightBulletOnCooldown = false;

    public float grenadeCooldown;
    private float currentGrenadeCooldown = 0;
    private bool grenadeOnCooldown = false;

    public GameObject lightBulletPrefab;

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

    private Camera cam;
    private Vector2 screenBounds;

    private bool isAiming = false;

    void Start() {
        cam = Camera.main;

        rb = GetComponent<Rigidbody2D>();
        light2D = GetComponent<Light2D>();
        light2D.pointLightOuterRadius = maxLightRadius;

        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //Cursor.visible = false;

        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        grenadeCooldownUI.SetMaxCooldown(grenadeCooldown);
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
            if (currentLightBulletCooldown < lightBulletCooldown) {
                currentLightBulletCooldown += Time.deltaTime;
            }
            else {
                lightBulletOnCooldown = false;
                currentLightBulletCooldown = 0;
            }
        }

        if (grenadeOnCooldown) {
            if (currentGrenadeCooldown < grenadeCooldown) {
                currentGrenadeCooldown += Time.deltaTime;
                grenadeCooldownUI.SetCooldown(currentGrenadeCooldown);
            }
            else {
                grenadeOnCooldown = false;
                currentGrenadeCooldown = 0;
                grenadeCooldownUI.SetCooldown(grenadeCooldown);
            }
        }
    }

    void FixedUpdate() {
        //Update Rigibody position
        Vector2 nextPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -screenBounds.x, screenBounds.x), Mathf.Clamp(nextPosition.y, -screenBounds.y, screenBounds.y)));
    }

    public void Shoot() {
        if (!lightBulletOnCooldown) {
            lightBulletOnCooldown = true;
            GameObject lightBullet = Instantiate(lightBulletPrefab);
            lightBullet.SetActive(true);
            lightBullet.GetComponent<LightBulletScript>().Shoot(crosshairInput);
        }
    }

    public void ThrowGrenade() {
        isAiming = false;
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
        crosshairInput = cam.ScreenToWorldPoint(new Vector3(input.x, input.y, 0));
    }

    public void SetIsAimingToTrue() {
        isAiming = true;
    }

}
