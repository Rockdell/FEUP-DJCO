using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableBehaviour : IBehaviour
{
    private const float ScrollSpeed = 10.0f;
    
    public void action(Rigidbody2D rigidbody)
    {
        Vector2 nextPosition = rigidbody.position + new Vector2(-1, 0) * (ScrollSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(new Vector2(nextPosition.x, rigidbody.position.y));
    }
}