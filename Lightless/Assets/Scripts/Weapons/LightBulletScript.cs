using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBulletScript : Entity
{
    public WeaponData weaponData;

    private Animator animator;
    private AnimationClip explosionAnimation;
    private float lightRadius;

    protected override void Awake() 
    {
        base.Awake();
        animator = GetComponent<Animator>();
        explosionAnimation = animator.runtimeAnimatorController.animationClips[1];
        lightRadius = GetComponentInChildren<Light2D>().pointLightOuterRadius;
    }

    void OnEnable() 
    {
        Spawn(GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position, Quaternion.identity);
    }

    void OnDisable()
    {
        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision) 
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

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("RedLight"))
        {
            collider.gameObject.GetComponent<RedLightScript>().ChangeHealth();
            EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("collided", true);
            yield return new WaitForSeconds(explosionAnimation.length);
            EntityBody.constraints = RigidbodyConstraints2D.None;
            gameObject.SetActive(false);
        }    
    }

    public void Shoot(Vector2 targetLocation) {
        EntityBody.AddForce((targetLocation - new Vector2(transform.position.x, transform.position.y)).normalized * weaponData.distancePerTimeUnit, ForceMode2D.Impulse);
    }
}
