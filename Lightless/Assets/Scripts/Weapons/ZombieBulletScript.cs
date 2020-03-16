using UnityEngine;

public class ZombieBulletScript : Entity
{
    public WeaponData weaponData;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.GetComponent<PlayerScript>().ChangeHealth(-weaponData.weaponDamage);
            gameObject.SetActive(false);
        }
        else if (collision.collider.CompareTag("ForceField")) {
            var direction = Vector3.Reflect(EntityBody.velocity, collision.contacts[0].normal).normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
            EntityBody.velocity = direction * weaponData.distancePerTimeUnit;
        }
        else
            gameObject.SetActive(false);
    }

    public void Shoot(Vector2 targetLocation)
    {
        var diff = targetLocation - new Vector2(transform.position.x, transform.position.y);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg, Vector3.forward);
        EntityBody.AddForce(diff.normalized * weaponData.distancePerTimeUnit, ForceMode2D.Impulse);
    }
}
