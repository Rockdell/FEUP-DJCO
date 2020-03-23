//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Experimental.Rendering.Universal;

//public class DarkBulletScript : Entity
//{
//    public WeaponData weaponData;

//    private Animator animator;
//    private AnimationClip explosionAnimation;
//    private float lightRadius;

//    protected override void Awake()
//    {
//        base.Awake();
//        animator = GetComponent<Animator>();
//        explosionAnimation = animator.runtimeAnimatorController.animationClips[1];
//        lightRadius = GetComponentInChildren<Light2D>().pointLightOuterRadius;
//    }

//    void OnEnable()
//    {
//        Spawn(GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position, Quaternion.identity);
//    }

//    void OnDisable()
//    {
//        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            collision.gameObject.GetComponent<PlayerScript>().ChangeHealth(-weaponData.weaponDamage);
//        }

//        StartCoroutine(CollisionEffect());
//    }

//    IEnumerator CollisionEffect()
//    {
//        EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
//        animator.SetBool("collided", true);
//        AudioManager.Instance.Stop("LightBulletShoot");
//        AudioManager.Instance.Play("LightBulletHit");
//        yield return new WaitForSeconds(explosionAnimation.length);
//        EntityBody.constraints = RigidbodyConstraints2D.None;
//        gameObject.SetActive(false);
//    }

//    public void Shoot(Vector2 targetLocation)
//    {
//        AudioManager.Instance.Play("LightBulletShoot");
//        EntityBody.AddForce((targetLocation - new Vector2(transform.position.x, transform.position.y)).normalized * weaponData.distancePerTimeUnit, ForceMode2D.Impulse);
//    }
//}
