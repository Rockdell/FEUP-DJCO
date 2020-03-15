using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public float weaponDamage;

    public float weaponCooldown;

    public float weaponRange;

    public float distancePerTimeUnit;
}
