using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowPointBehaviour : IBehaviour
{
    private Vector2 targetPoint;
    private float flowSpeed;

    public FlowPointBehaviour(Vector2 point, float speed)
    {
        targetPoint = point;
        flowSpeed = speed;
    }

    public void Action(Rigidbody2D rigidbody)
    {
        if (Vector2.Distance(rigidbody.position, targetPoint) > 0.5f)
        {
            Vector2 direction = targetPoint - rigidbody.position;
            rigidbody.MovePosition(rigidbody.position + direction.normalized * (flowSpeed * Time.fixedDeltaTime));
        }
    }
}
