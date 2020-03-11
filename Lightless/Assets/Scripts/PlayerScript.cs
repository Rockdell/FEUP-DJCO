using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {

    [Header("Player Stats")]
    public float moveSpeed;
    public int maxHealth;
    public int healthLossPerSecond;
    private float currentHealth;

    [Header("Player Options")]
    public HealthBarScript healthBar;
    public GameObject crosshair;
    public GameObject grenadeHolder;
    public GameObject grenadeStart;
    private Rigidbody2D rb;

    /* Trajectory */
    [Header("Trajectory Settings")]
    public LineRenderer lineVisual;
    public int linePrecision;

    /* Inputs */
    private Vector2 movementInput;
    private Vector2 crosshairInput;

    private Camera cam;
    private Vector2 screenBounds;

    private bool isAiming;

    void Start() {
        cam = Camera.main;
        isAiming = false;
        rb = GetComponent<Rigidbody2D>();
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //Cursor.visible = false;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update() {
        crosshair.transform.position = new Vector2(crosshairInput.x, crosshairInput.y);

        //if (isAiming)
        //    DrawTrajectory();
    }

    void FixedUpdate() {
        //Update Rigibody position
        Vector2 nextPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -screenBounds.x, screenBounds.x), Mathf.Clamp(nextPosition.y, -screenBounds.y, screenBounds.y)));

        //Update HP (Light)
        if (currentHealth <= 0)
            Debug.Log("You have Corona, Im sorry you're dead.");
        else {
            currentHealth -= healthLossPerSecond * Time.fixedDeltaTime;
            healthBar.SetHealth((int)currentHealth);
        }
    }

    public void Shoot() {
        isAiming = false;
        lineVisual.positionCount = 0;

        GameObject grenade = GameManager.instance.GetGrenade();
        grenade.SetActive(true);
        grenade.GetComponent<GrenadeScript>().Throw(crosshairInput);
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
