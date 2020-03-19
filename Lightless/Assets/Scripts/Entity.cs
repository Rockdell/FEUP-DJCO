using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Rigidbody2D EntityBody { get; private set; }
    protected IBehaviour Behaviour { get; set; }

    protected virtual void Awake()
    {
        EntityBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
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
