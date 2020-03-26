using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DarkBulletScript : Entity
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
        //lightRadius = GetComponentInChildren<Light2D>().pointLightOuterRadius;
    }

    void OnEnable()
    {
        gameObject.layer = 11;
    }

    //void OnDisable()
    //{
    //    GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    //}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().ChangeHealth(-weaponData.weaponDamage);
            StartCoroutine(CollisionEffect());
        }
        else if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieScript>().ChangeHealth(-weaponData.weaponDamage);
            StartCoroutine(CollisionEffect());
        }
        else if (collision.gameObject.CompareTag("Firefly"))
        {
            collision.gameObject.GetComponent<FireflyScript>().ChangeHealth(-weaponData.weaponDamage);
            StartCoroutine(CollisionEffect());
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossScript>().ChangeHealth(-weaponData.weaponDamage);
            StartCoroutine(CollisionEffect());
        }
        else if (collision.collider.CompareTag("ForceField"))
        {
            var direction = Vector3.Reflect(EntityBody.velocity, collision.contacts[0].normal).normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
            EntityBody.velocity = direction * weaponData.distancePerTimeUnit;
            AudioManager.Instance.Play("ShieldDeflect");

            gameObject.layer = 9;
        }
        else
        {
            StartCoroutine(CollisionEffect());
        }
    }

    IEnumerator CollisionEffect()
    {
        //EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("collided", true);
        AudioManager.Instance.Stop("LightBulletShoot");
        //AudioManager.Instance.Play("LightBulletHit");
        yield return new WaitForSeconds(explosionAnimation.length);
        //EntityBody.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }

    public void Shoot(Vector2 targetLocation)
    {
        AudioManager.Instance.Play("LightBulletShoot");
        EntityBody.AddForce((targetLocation - new Vector2(transform.position.x, transform.position.y)).normalized * weaponData.distancePerTimeUnit, ForceMode2D.Impulse);
    }
}
