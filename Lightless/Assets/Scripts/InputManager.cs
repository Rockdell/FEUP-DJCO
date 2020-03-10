using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private InputActions inputActions;
    private PlayerScript player;

    private void Awake() {
        inputActions = new InputActions();
        player = GetComponent<PlayerScript>();
        inputActions.Player.Move.performed += ctx => player.UpdateMovement(ctx.ReadValue<Vector2>());
        inputActions.Player.Look.performed += ctx => player.UpdateCrosshair(ctx.ReadValue<Vector2>());
        inputActions.Player.Fire.performed += _ => player.SetIsAimingToTrue();
        inputActions.Player.Fire.canceled += _ => player.Shoot();
    }

    private void OnEnable() {
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }
}
