using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GrenadeScript : Entity {

    public WeaponData weaponData;

    //public float distancePerTimeUnit;
    public float minTime;
    private Animator animator;
    private AnimationClip explosionAnimation;
    private float lightRadius;

    void Start() {
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
    }

    void OnDisable() {
        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieScript>().ChangeHealth(-weaponData.weaponDamage);
        }
        else if (collision.gameObject.CompareTag("Firefly"))
        {
            collision.gameObject.GetComponent<FireflyScript>().ChangeHealth(-weaponData.weaponDamage);
        }

        EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("collided", true);
        yield return new WaitForSeconds(explosionAnimation.length);
        EntityBody.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }

    public void Throw(Vector2 targetLocation) {
        EntityBody.AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / weaponData.distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
    }
}
