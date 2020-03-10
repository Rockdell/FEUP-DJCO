using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private ObjectPool obstacles;

    void Start()
    {
        obstacles = new ObjectPool(obstaclePrefab, transform);
        InvokeRepeating("Spawn", 1.0f, 1.0f);
    }

    void Spawn()
    {
        GameObject obj = obstacles.GetObject();
        obj.GetComponent<ObstacleScript>().Spawn(new Vector3(70.0f, 10.0f, 0.0f), Quaternion.identity);
        obj.SetActive(true);
    }
}
