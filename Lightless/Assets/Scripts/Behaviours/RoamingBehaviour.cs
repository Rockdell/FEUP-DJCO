using UnityEngine;

public class RoamingBehaviour : IBehaviour
{
    private float roamingSpeed;
    private readonly Vector2 lowerCorner = new Vector2(-GameManager.Instance.screenBounds.x, -GameManager.Instance.screenBounds.y);
    private readonly Vector2 upperCorner = new Vector2(GameManager.Instance.screenBounds.x, GameManager.Instance.screenBounds.y);

    private Vector2 direction;

    public RoamingBehaviour(float speed)
    {
        roamingSpeed = speed;

        float angle = Random.Range(135.0f, 225.0f) * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public void Action(Rigidbody2D rigidbody)
    {
        float size = 3.5f;

        //Following formula  v' = 2 * (v . n) * n - v

        if (rigidbody.position.y + size > upperCorner.y || rigidbody.position.y - size < lowerCorner.y)    // Top wall / Bottom wall
        {
            direction = 2 * (Vector3.Dot(direction, Vector2.up)) * Vector2.up - direction;
            direction *= -1;

            rigidbody.MovePosition(rigidbody.position + direction.normalized * (roamingSpeed * Time.fixedDeltaTime));
        }
        else if (rigidbody.position.x - size < lowerCorner.x || rigidbody.position.x + size > upperCorner.x)    // Left wall / Right wall
        {
            direction = 2 * (Vector3.Dot(direction, Vector2.right)) * Vector2.right - direction;
            direction *= -1;

            rigidbody.MovePosition(rigidbody.position + direction.normalized * (roamingSpeed * Time.fixedDeltaTime));
        }
        else
        {
            rigidbody.MovePosition(rigidbody.position + direction.normalized * (roamingSpeed * Time.fixedDeltaTime));
        }


    }
}
