using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    public float distancePerTimeUnit;
    public float minTime;

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
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
