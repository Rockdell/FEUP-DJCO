using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : Entity
{
    void Start()
    {
        AddBehaviour("scroll", new ScrollableBehaviour());
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Grenade") || collision.gameObject.CompareTag("Boundary"))
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
