using UnityEngine;

public class HomingBehaviour : IBehaviour
{
    private float speed = 15.0f;

    public void Action(Rigidbody2D rigidbody)
    {
        Vector2 direction = (Vector2)GameManager.Instance.GetPlayer().transform.position - rigidbody.position;
        rigidbody.velocity = direction.normalized * speed;
    }
}
