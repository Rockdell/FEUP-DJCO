using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBulletScript : MonoBehaviour
{
    public float distancePerTimeUnit;
    private Animator animator;
    private AnimationClip explosionAnimation;

    void Awake() {
        animator = GetComponent<Animator>();
        explosionAnimation = animator.runtimeAnimatorController.animationClips[1];
    }

    void OnEnable() {
        transform.position = GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position;
    }

    public void Shoot(Vector2 targetLocation) {
        GetComponent<Rigidbody2D>().AddForce((targetLocation - new Vector2(transform.position.x, transform.position.y)).normalized * distancePerTimeUnit, ForceMode2D.Impulse);
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision) {

        animator.SetBool("collided", true);

        yield return new WaitForSeconds(explosionAnimation.length);

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

    // Update is called once per frame
    void Update() {

    }
}
