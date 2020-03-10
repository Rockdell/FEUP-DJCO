using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 15f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 nextPosition = rb.position + new Vector2(-1.0f, 0.0f) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(nextPosition.x, 0.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary") || collision.gameObject.CompareTag("Grenade"))
        {
            gameObject.SetActive(false);
        }
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
