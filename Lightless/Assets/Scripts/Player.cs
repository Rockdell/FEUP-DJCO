using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveForwardSpeed;
    public float moveSpeed;
    public Rigidbody2D rb;

    private InputActions inputActions;
    private Vector2 movementInput;
    private Vector2 screenBounds;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    }

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 nextPosition = rb.position + (new Vector2(moveForwardSpeed, 0f) + movementInput * moveSpeed) * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(Mathf.Clamp(nextPosition.x, -screenBounds.x, screenBounds.x), Mathf.Clamp(nextPosition.y, -screenBounds.y, screenBounds.y)));
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
