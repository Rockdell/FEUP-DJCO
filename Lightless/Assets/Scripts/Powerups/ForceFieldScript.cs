using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldScript : MonoBehaviour {

    public PowerUpData forceFieldData;

    private CooldownScript powerUpUI;
    private float remainingForceFieldTime;
    private PlayerScript player;
    private SpriteRenderer spriteRenderer;
    private EdgeCollider2D edgeCollider;

    private void Awake() {
        remainingForceFieldTime = forceFieldData.duration;
        spriteRenderer = GetComponent<SpriteRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        player = GetComponentInParent<PlayerScript>();
        Hide();
        powerUpUI = GameObject.FindGameObjectWithTag("PowerUpUI").GetComponent<CooldownScript>();
        powerUpUI.SetMaxCooldown(forceFieldData.duration, true);
    }

    void FixedUpdate() {

        if (remainingForceFieldTime <= 0) {
            powerUpUI.SetCooldown(0f);
            Destroy(gameObject);
        }

        if (player.GetPowerUpInput()) {
            Reveal();
            remainingForceFieldTime -= Time.fixedDeltaTime;
            powerUpUI.SetCooldown(remainingForceFieldTime);
        }
        else
            Hide();

        var diff = player.GetCrosshairInput() - new Vector2(transform.position.x, transform.position.y);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    private void Hide() {
        spriteRenderer.enabled = false;
        edgeCollider.enabled = false;
    }

    private void Reveal() {
        spriteRenderer.enabled = true;
        edgeCollider.enabled = true;
    }
}
