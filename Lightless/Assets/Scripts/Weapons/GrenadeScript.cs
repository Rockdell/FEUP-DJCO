﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GrenadeScript : Entity {

    public WeaponData weaponData;

    //public float distancePerTimeUnit;
    public float minTime;
    private Animator animator;
    private AnimationClip explosionAnimation;
    private float lightRadius;

    private Dictionary<int, bool> enemiesHit;

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
        enemiesHit = new Dictionary<int, bool>();
    }

    void OnDisable() {
        GetComponentInChildren<Light2D>().pointLightOuterRadius = lightRadius;
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("collided", true);
        yield return new WaitForSeconds(explosionAnimation.length);
        gameObject.SetActive(false);
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

    public void Throw(Vector2 targetLocation) {
        EntityBody.AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / weaponData.distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
    }
}
