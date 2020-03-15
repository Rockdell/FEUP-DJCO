using UnityEngine;

public class DropLightScript : Entity
{
    void Start()
    {
        SetBehaviour(new FloatBehaviour());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
