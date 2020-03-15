using UnityEngine;

public class EnemyScript : Entity
{
    void Start()
    {
        SetBehaviour(new ScrollableBehaviour());
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Grenade") || collision.gameObject.CompareTag("Boundary"))
        {
            gameObject.SetActive(false);
        }
    }
}
