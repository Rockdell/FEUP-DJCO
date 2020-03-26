using UnityEngine;

public class ZombieBulletScript : Entity
{
    public WeaponData weaponData;

    void OnEnable()
    {
        gameObject.layer = 11;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            collision.collider.GetComponent<PlayerScript>().ChangeHealth(-weaponData.weaponDamage);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.collider.GetComponent<ZombieScript>().ChangeHealth(-weaponData.weaponDamage);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Firefly"))
        {
            collision.collider.GetComponent<FireflyScript>().ChangeHealth(-weaponData.weaponDamage);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossScript>().ChangeHealth(-weaponData.weaponDamage);
            gameObject.SetActive(false);
        }
        else if (collision.collider.CompareTag("ForceField")) 
        {
            var direction = Vector3.Reflect(EntityBody.velocity, collision.contacts[0].normal).normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
            EntityBody.velocity = direction * weaponData.distancePerTimeUnit;
            AudioManager.Instance.Play("ShieldDeflect");

            gameObject.layer = 9;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Shoot(Vector2 targetLocation)
    {
        Vector2 direction = targetLocation - (Vector2)transform.position;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
        EntityBody.AddForce(direction.normalized * weaponData.distancePerTimeUnit, ForceMode2D.Impulse);
    }
}
