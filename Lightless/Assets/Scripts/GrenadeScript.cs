using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GrenadeScript : Entity {

    public float distancePerTimeUnit;
    public float minTime;
    private Animator animator;
    private AnimationClip explosionAnimation;
    private float lightRadius;

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
    }

    void OnDisable() {
        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision) {

        EntityBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("collided", true);
        yield return new WaitForSeconds(explosionAnimation.length);
        EntityBody.constraints = RigidbodyConstraints2D.None;

        //if (collision.gameObject.CompareTag("Boundary")) {
            gameObject.SetActive(false);
            
            //if (collision.gameObject.name == "Boundary Bottom")
            //{
            //    GameObject grenadeFire = GameManager.Instance.GetObject(GameManager.ObjectType.GrenadeFire);
            //    grenadeFire.GetComponent<GrenadeFireScript>().Spawn(collision.GetContact(0).point, Quaternion.identity);
            //    grenadeFire.SetActive(true);
            //}
        //}
    }
    
    public void Throw(Vector2 targetLocation) 
    {
        EntityBody.AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
    }
}
