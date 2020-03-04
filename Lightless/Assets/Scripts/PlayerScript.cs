using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {

    public float moveSpeed;
    public Rigidbody2D rb;
    public GameObject crosshair;
    public GameObject grenadePrefab;
    public GameObject grenadeStart;
    public float horizontalVelocity;

    /* Trajectory */
    public LineRenderer lineVisual;
    public int linePrecision;

    /* Inputs */
    private InputActions inputActions;
    private Vector2 movementInput;
    private Vector2 crosshairInput;

    private Camera cam;
    private Vector2 screenBounds;

    private bool isAiming;

    private void Awake() {
        cam = Camera.main;
        isAiming = false;
        inputActions = new InputActions();
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.performed += ctx => {
            Vector2 temp = ctx.ReadValue<Vector2>();
            crosshairInput = cam.ScreenToWorldPoint(new Vector3(temp.x, temp.y, transform.position.z));
        };
        inputActions.Player.Fire.performed += ctx => {
            isAiming = true;
            lineVisual.positionCount = linePrecision;
        };
        inputActions.Player.Fire.canceled += ctx => Shoot();
    }

    void Start() {
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        //Vector3 target = cam.ScreenToWorldPoint(new Vector3(crosshairInput.x, crosshairInput.y, transform.position.z));
        crosshair.transform.position = new Vector2(crosshairInput.x, crosshairInput.y);

        if (isAiming)
            DrawTrajectory();
    }

    void FixedUpdate() {
        Vector2 nextPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -screenBounds.x, screenBounds.x), Mathf.Clamp(nextPosition.y, -screenBounds.y, screenBounds.y)));
    }

    private void Shoot() {
        isAiming = false;
        lineVisual.positionCount = 0;

        Instantiate(grenadePrefab).GetComponent<GrenadeScript>().Throw(crosshairInput);
    }

    private void DrawTrajectory() {
        float timePerSegment = (crosshairInput.x - grenadeStart.transform.position.x) / (horizontalVelocity * linePrecision);
        Vector3 initialPosition = grenadeStart.transform.position;
        for (int i = 0; i < linePrecision; i++) {
            lineVisual.SetPosition(i, Tools.CalculatePositionInTime(initialPosition, horizontalVelocity, Tools.CalculateVerticalVelocity(initialPosition, crosshairInput, horizontalVelocity), i * timePerSegment));
        }
    }

    private void OnEnable() {
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }
}
