using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : Entity {

    public float distancePerTimeUnit;
    public float minTime;

    // public GameObject firePrefab;
    
    void Start()
    {
        SetBehaviour(new ArchBehaviour());
    }

    void OnEnable() 
    {
        Spawn(GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position, Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Boundary")) 
        {
            gameObject.SetActive(false);
            
            if (collision.gameObject.name == "Boundary Bottom")
            {
                GameObject grenadeFire = GameManager.Instance.GetObject(GameManager.ObjectType.GrenadeFire);
                grenadeFire.GetComponent<GrenadeFireScript>().Spawn(new Vector3(38, -12 + Random.Range(0, 5)), Quaternion.identity);
                grenadeFire.SetActive(true);
            }
        }
    }
    
    public void Throw(Vector2 targetLocation) 
    {
        EntityBody.AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
    }
}
