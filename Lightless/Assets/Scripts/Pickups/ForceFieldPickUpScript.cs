using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldPickUpScript : Entity {

    public GameObject forceFieldPrefab;

    void Start() 
    {
        SetBehaviour(new ScrollableBehaviour(10f));
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (collision.gameObject.GetComponentInChildren<ForceFieldScript>() == null) 
            {
                Instantiate(forceFieldPrefab, collision.gameObject.transform);
                AudioManager.Instance.Play("PowerUpCatch");
                Destroy(gameObject);
            }
        }
    }
}
