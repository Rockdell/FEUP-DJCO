using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBulletScript : Entity
{
    public float distancePerTimeUnit;
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

    void OnEnable() {
        Spawn(GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position, Quaternion.identity);
    }

    void OnDisable() {
        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision) {

        EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("collided", true);
        yield return new WaitForSeconds(explosionAnimation.length);
        EntityBody.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);

        //if (collision.gameObject.CompareTag("Boundary")) {
        //    gameObject.SetActive(false);
        //    //Destroy(gameObject);
        //    //Debug.Log(collision.GetContact(0).point);
        //    if (collision.gameObject.name == "Boundary Bottom") {
        //        GameObject gf = Instantiate(grenadeFire);
        //        gf.transform.position = collision.GetContact(0).point;
        //    }
        //}
    }
    
    public void Shoot(Vector2 targetLocation) {
        EntityBody.AddForce((targetLocation - new Vector2(transform.position.x, transform.position.y)).normalized * distancePerTimeUnit, ForceMode2D.Impulse);
    }
}
