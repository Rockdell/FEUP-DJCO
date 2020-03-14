using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchBehaviour : IBehaviour
{
    public void Action(Rigidbody2D entityBody)
    {
        Vector2 vel2D = entityBody.velocity;
        entityBody.MoveRotation(Quaternion.AngleAxis(Mathf.Atan2(vel2D.y, vel2D.x) * Mathf.Rad2Deg, Vector3.forward));
    }
}
