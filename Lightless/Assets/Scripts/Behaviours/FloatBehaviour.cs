using UnityEngine;

class FloatBehaviour : IBehaviour
{
    private const float Speed = 10.0f;

   public void Action(Rigidbody2D rigidbody)
   {
        Vector2 direction = new Vector2(-1.0f, (1.0f/2.0f) * Mathf.Cos(Time.fixedTime));
        rigidbody.MovePosition(rigidbody.position + direction * (Speed * Time.fixedDeltaTime));
    }

}
