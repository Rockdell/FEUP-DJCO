using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    public float distancePerTimeUnit;
    public float minTime;
    public GameObject grenadeFire;

    void OnEnable() {
        transform.position = GameObject.FindGameObjectWithTag("GrenadeStart").transform.position;
    }

    public void Throw(Vector2 targetLocation) {
        GetComponent<Rigidbody2D>().AddForce(Tools.CalculateVelocity(transform.position, targetLocation, (Mathf.Abs(transform.position.x - targetLocation.x) / distancePerTimeUnit) + minTime), ForceMode2D.Impulse);
        //GetComponent<Rigidbody2D>().AddTorque(50f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {

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

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector2 vel2D = GetComponent<Rigidbody2D>().velocity;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(vel2D.y, vel2D.x) * Mathf.Rad2Deg, Vector3.forward);
    }
}
