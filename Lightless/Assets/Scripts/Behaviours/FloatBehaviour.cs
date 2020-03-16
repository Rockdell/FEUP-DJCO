using UnityEngine;

class FloatBehaviour : IBehaviour
{
    private float floatingSpeed;

    public FloatBehaviour(float speed)
    {
        floatingSpeed = speed;
    }

    public void Action(Rigidbody2D rigidbody)
    {
        // f(x) = 1/2 * sin(x)
        Vector2 direction = new Vector2(-1.0f, (1.0f/2.0f) * Mathf.Cos(Time.fixedTime));
        rigidbody.MovePosition(rigidbody.position + direction * (floatingSpeed * Time.fixedDeltaTime));
    }
}
