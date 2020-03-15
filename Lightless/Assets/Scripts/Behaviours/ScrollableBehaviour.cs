using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableBehaviour : IBehaviour 
{
    public float scrollSpeed { get; set; }

    public ScrollableBehaviour(float speed = 10.0f)
    {
        scrollSpeed = speed;
    }
    
    public void Action(Rigidbody2D rigidbody)
    {
        Vector2 nextPosition = rigidbody.position + Vector2.left * (scrollSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(new Vector2(nextPosition.x, rigidbody.position.y));
    }
}