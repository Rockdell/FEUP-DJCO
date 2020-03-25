using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnScript : MonoBehaviour {

    public PowerUpData forceFieldData;
    public GameObject burnEffectPrefab;

    private float burnAnimationTime;
    private CooldownScript powerUpUI;
    private float remainingBurnTime;

    public float healthGiven;
    public float burnDamage;
    public float burnCooldown;
    private float currentBurnCooldown = 0;

    private void Awake() {
        remainingBurnTime = forceFieldData.duration;
        powerUpUI = GameObject.FindGameObjectWithTag("BurnPowerUpUI").GetComponent<CooldownScript>();
        powerUpUI.SetMaxCooldown(forceFieldData.duration, true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().ChangeHealth(healthGiven);
        burnAnimationTime = burnEffectPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
    }

    void FixedUpdate() {

        if (remainingBurnTime <= 0) {
            powerUpUI.SetCooldown(0f);
            Destroy(gameObject);
        }

        remainingBurnTime -= Time.fixedDeltaTime;
        powerUpUI.SetCooldown(remainingBurnTime);

        if (currentBurnCooldown > 0)
            currentBurnCooldown -= Time.fixedDeltaTime;
        else
            currentBurnCooldown = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageNearbyEnemies(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        DamageNearbyEnemies(collision);
    }

    private void DamageNearbyEnemies(Collider2D collision) {
        if (currentBurnCooldown <= 0) {
            if (collision.gameObject.CompareTag("Zombie")) {
                collision.gameObject.GetComponent<ZombieScript>().ChangeHealth(-burnDamage);
                currentBurnCooldown = burnCooldown;
            }
            else if (collision.gameObject.CompareTag("Firefly")) {
                collision.gameObject.GetComponent<FireflyScript>().ChangeHealth(-burnDamage);
                currentBurnCooldown = burnCooldown;
            }
            else if (collision.gameObject.CompareTag("Boss")) {
                collision.gameObject.GetComponent<BossScript>().ChangeHealth(-burnDamage);
                currentBurnCooldown = burnCooldown;
            }

            if (currentBurnCooldown == burnCooldown) {
                GameObject burn = Instantiate(burnEffectPrefab, collision.gameObject.transform.GetChild(0));
                Vector3 parentScale = burn.gameObject.transform.parent.transform.parent.localScale;
                Vector3 burnScale = burn.gameObject.transform.localScale;
                burn.gameObject.transform.localScale = new Vector3(burnScale.x / parentScale.x, burnScale.y / parentScale.y, burnScale.z / parentScale.z);
                Destroy(burn, burnAnimationTime);
            }
        }
    }

}
