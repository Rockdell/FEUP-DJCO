using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableBehaviour : IBehaviour
{
    private const float ScrollSpeed = 10.0f;
    
    public void Action(Rigidbody2D rigidbody)
    {
        Vector2 nextPosition = rigidbody.position + Vector2.left * (ScrollSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(new Vector2(nextPosition.x, rigidbody.position.y));
    }
}