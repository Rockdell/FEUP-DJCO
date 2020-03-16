using UnityEngine;

public class HomingBehaviour : IBehaviour
{
    private float homingSpeed;

    public HomingBehaviour(float speed)
    {
        homingSpeed = speed;
    }

    public void Action(Rigidbody2D rigidbody)
    {
        Vector2 direction = (Vector2)GameManager.Instance.GetPlayer().transform.position - rigidbody.position;
        rigidbody.velocity = direction.normalized * homingSpeed;
    }
}
