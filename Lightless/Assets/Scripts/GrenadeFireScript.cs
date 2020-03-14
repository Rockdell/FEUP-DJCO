using UnityEngine;

public class GrenadeFireScript : Entity
{
    void Start()
    {
        SetBehaviour(new ScrollableBehaviour());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Boundary"))
        {
            if (collider.gameObject.name == "Boundary Left" || collider.gameObject.name == "Boundary Top")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
