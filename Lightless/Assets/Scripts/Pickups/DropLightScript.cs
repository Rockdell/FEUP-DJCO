using UnityEngine;

public class DropLightScript : Entity
{
    private const float healthRegen = 20.0f;

    void Start()
    {
        SetBehaviour(new FloatBehaviour(10.0f));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().ChangeHealth(healthRegen);
        }

        gameObject.SetActive(false);
    }
}
