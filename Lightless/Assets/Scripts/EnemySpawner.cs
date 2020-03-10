using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private ObjectPool enemies;

    void Start()
    {
        enemies = new ObjectPool(enemyPrefab, transform);
        InvokeRepeating("Spawn", 1.0f, 1.0f);
    }

    void Spawn()
    {
        GameObject obj = enemies.GetObject();
        obj.GetComponent<EnemyScript>().Spawn(new Vector3(38, -12), Quaternion.identity);
        obj.SetActive(true);
    }
}
