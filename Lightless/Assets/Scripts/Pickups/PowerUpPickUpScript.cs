using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUpScript : Entity {

    public enum PowerUpType {
        ForceShield, BurnEffect
    };

    public PowerUpType powerUp;
    public GameObject powerUpPrefab;

    // Start is called before the first frame update
    void Start() {
        SetBehaviour(new ScrollableBehaviour(10f));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {

            bool doesPlayerHavePowerUp = false;
            if (powerUp == PowerUpType.ForceShield)
                doesPlayerHavePowerUp = (collision.gameObject.GetComponentInChildren<ForceFieldScript>() != null);
            else if (powerUp == PowerUpType.BurnEffect) {
                doesPlayerHavePowerUp = (collision.gameObject.GetComponentInChildren<BurnScript>() != null);
            }

            if (!doesPlayerHavePowerUp) {
                Instantiate(powerUpPrefab, collision.gameObject.transform);
                AudioManager.Instance.Play("PowerUpCatch");
                gameObject.SetActive(false);
            }
        }
    }
}
