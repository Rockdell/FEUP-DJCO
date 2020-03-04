using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    public float horizontalVelocity;

    void Awake() {
        transform.position = GameObject.FindGameObjectWithTag("GrenadeStart").transform.position;
    }

    public void Throw(Vector2 targetLocation) {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalVelocity, Tools.CalculateVerticalVelocity(transform.position, targetLocation, horizontalVelocity)), ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddTorque(50f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Boundary")) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
