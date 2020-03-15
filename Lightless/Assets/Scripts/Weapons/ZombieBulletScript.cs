using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBulletScript : Entity
{
    public WeaponData weaponData;

    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }

    public void Shoot(Vector2 targetLocation)
    {
        var diff = targetLocation - new Vector2(transform.position.x, transform.position.y);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg, Vector3.forward);
        EntityBody.AddForce(diff.normalized * weaponData.distancePerTimeUnit, ForceMode2D.Impulse);
    }
}
