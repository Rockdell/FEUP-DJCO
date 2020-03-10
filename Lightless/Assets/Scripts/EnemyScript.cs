using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 nextPosition = rb.position + new Vector2(-1, 0) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(nextPosition.x, rb.position.y));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Grenade") || collision.gameObject.CompareTag("Boundary"))
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
