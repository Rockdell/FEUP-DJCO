using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : Entity
{
    private float currentHealth;
    public float targetPosition { get; set; }

    private Animator animator;
    private AnimationClip deathAnimation;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        deathAnimation = animator.runtimeAnimatorController.animationClips[1];
    }

    void OnEnable()
    {
        currentHealth = 100.0f;
        Behaviour = new ScrollableBehaviour();
    }

    void Update()
    {
        // TODO Improve later
        if (EntityBody.position.x - targetPosition <= Mathf.Epsilon)
        {
            var runningFromPlayer = new ScrollableBehaviour();
            runningFromPlayer.scrollSpeed = 0f;
            Behaviour = runningFromPlayer;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            var runningToPlayer = new ScrollableBehaviour();
            Behaviour = runningToPlayer;
        }
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightBullet"))     // LightBullet
        {
            currentHealth -= 20.0f;
        }
        else if (collision.gameObject.CompareTag("Grenade"))    // Grenade
        {
            currentHealth -= 50.0f;
        }

        // Check health
        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(deathAnimation.length);
            gameObject.SetActive(false);
        }
    }
}
