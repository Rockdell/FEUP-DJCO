using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour {

    private const float moveForwardSpeed = 20f;
    private float length, startpos;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start() {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (transform.position.x < startpos - length)
            transform.position = new Vector3(startpos, transform.position.y, transform.position.z);

        transform.position -= new Vector3(parallaxEffect * moveForwardSpeed * Time.fixedDeltaTime, 0f, 0f);
    }

}
