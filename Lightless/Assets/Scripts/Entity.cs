using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Rigidbody2D EntityBody { get; private set; }
    private IBehaviour Behaviour;

    protected virtual void Awake()
    {
        EntityBody = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        Behaviour?.Action(EntityBody);
    }

    protected void SetBehaviour(IBehaviour behaviour)
    {
        Behaviour = behaviour;
    }
    
    /**
     * Spawns entity with the given position and rotation
     */
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
