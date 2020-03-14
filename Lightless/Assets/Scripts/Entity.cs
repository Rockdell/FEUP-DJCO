using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Rigidbody2D Rigidbody { get; private set; }
    private Dictionary<string, IBehaviour> Behaviours;

    protected virtual void Awake()
    {
        Behaviours = new Dictionary<string, IBehaviour>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        // Execute attached behaviours
        foreach (var entry in Behaviours)
        {
            entry.Value.action(Rigidbody);
        }
    }

    /**
     * Spawns entity with the given position and rotation
     */
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    protected void AddBehaviour(string behaviourTag, IBehaviour behaviour)
    {
        Behaviours.Add(behaviourTag, behaviour);
    }

    protected void RemoveBehaviour(string behaviourTag)
    {
        Behaviours.Remove(behaviourTag);
    }
}
