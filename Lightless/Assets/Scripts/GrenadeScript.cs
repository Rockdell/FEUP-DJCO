using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : Entity {

    public float distancePerTimeUnit;
    public float minTime;
    public GameObject grenadeFire;

    void Start()
    {
        SetBehaviour(new ArchBehaviour());
    }
    
    void OnEnable() {
        Spawn(GameObject.FindGameObjectWithTag("PlayerProjectileStart").transform.position, Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Boundary")) {
            gameObject.SetActive(false);
            //Destroy(gameObject);
            //Debug.Log(collision.GetContact(0).point);
            if (collision.gameObject.name == "Boundary Bottom") {
                GameObject gf = Instantiate(grenadeFire);
                gf.transform.position = collision.GetContact(0).point;
            }
        }
    }
    
    public void Throw(Vector2 targetLocation) {
        EntityBody.AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
    }
}
