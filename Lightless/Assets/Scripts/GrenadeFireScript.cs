using UnityEngine;

public class GrenadeFireScript : Entity
{
    void Start()
    {
        SetBehaviour(new ScrollableBehaviour());       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary")) {
    
            if (collision.gameObject.name == "Boundary Left" || collision.gameObject.name == "Boundary Top")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
