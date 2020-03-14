using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : Entity
{
    void Start()
    {
        AddBehaviour("scroll", new ScrollableBehaviour());
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary") || collision.gameObject.CompareTag("Grenade"))
        {
            gameObject.SetActive(false);
        }
    }
}
