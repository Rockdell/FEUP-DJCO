using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GrenadeScript : Entity {

    public WeaponData weaponData;

    public float minTime;
    private Animator animator;
    private AnimationClip explosionAnimation;
    private float lightRadius;

    private Dictionary<int, bool> enemiesHit;

    void Start() 
    {
        SetBehaviour(new ArchBehaviour());
    }

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        explosionAnimation = animator.runtimeAnimatorController.animationClips[1];
        lightRadius = GetComponentInChildren<Light2D>().pointLightOuterRadius;
    }

    void OnEnable() {
        Spawn(GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position, Quaternion.identity);
        enemiesHit = new Dictionary<int, bool>();
    }

    void OnDisable() {
        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(CollisionEffect());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!enemiesHit.ContainsKey(collider.GetInstanceID()) && collider.gameObject.CompareTag("Zombie"))
        {
            collider.gameObject.GetComponent<ZombieScript>().ChangeHealth(-weaponData.weaponDamage);
            enemiesHit.Add(collider.GetInstanceID(), true);
        }
        else if (!enemiesHit.ContainsKey(collider.GetInstanceID()) && collider.gameObject.CompareTag("Firefly"))
        {
            collider.gameObject.GetComponent<FireflyScript>().ChangeHealth(-weaponData.weaponDamage);
            enemiesHit.Add(collider.GetInstanceID(), true);
        }
        else if (!enemiesHit.ContainsKey(collider.GetInstanceID()) && collider.gameObject.CompareTag("RedLight"))
        {
            StartCoroutine(CollisionEffect());
            collider.gameObject.GetComponent<RedLightScript>().ChangeHealth(true);
            enemiesHit.Add(collider.GetInstanceID(), true);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!enemiesHit.ContainsKey(collider.GetInstanceID()) && collider.gameObject.CompareTag("Zombie"))
        {
            collider.gameObject.GetComponent<ZombieScript>().ChangeHealth(-weaponData.weaponDamage);
            enemiesHit.Add(collider.GetInstanceID(), true);
        }
        else if (!enemiesHit.ContainsKey(collider.GetInstanceID()) && collider.gameObject.CompareTag("Firefly"))
        {
            collider.gameObject.GetComponent<FireflyScript>().ChangeHealth(-weaponData.weaponDamage);
            enemiesHit.Add(collider.GetInstanceID(), true);
        }
        else if (!enemiesHit.ContainsKey(collider.GetInstanceID()) && collider.gameObject.CompareTag("RedLight")) 
        {
            StartCoroutine(CollisionEffect());
            collider.gameObject.GetComponent<RedLightScript>().ChangeHealth(true);
            enemiesHit.Add(collider.GetInstanceID(), true);
        }
    }

    IEnumerator CollisionEffect()
    {
        animator.SetBool("collided", true);
        AudioManager.Instance.Stop("GrenadeThrow");
        AudioManager.Instance.Play("Explosion");
        yield return new WaitForSeconds(explosionAnimation.length);
        gameObject.SetActive(false);
    }

    public void Throw(Vector2 targetLocation) {
        AudioManager.Instance.Play("GrenadeThrow");
        EntityBody.AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / weaponData.distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
    }
}
