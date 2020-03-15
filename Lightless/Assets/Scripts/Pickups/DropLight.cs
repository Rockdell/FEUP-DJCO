using UnityEngine;

public class DropLight : Entity
{
    void Start()
    {
        SetBehaviour(new FloatBehaviour());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
