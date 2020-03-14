using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Dictionary<string, ObjectPool> objectPools;
    
    public GameObject enemyPrefab;
    public GameObject obstaclePrefab;
    
    // Move somewhere else
    public GameObject grenadePrefab;
    public ObjectPool grenadePool;

    void Awake()
    {
        instance = this;
        
        // Initialize object pools
        objectPools = new Dictionary<string, ObjectPool>();
        AddPool(enemyPrefab);
        AddPool(obstaclePrefab);
    }

    void Start()
    {
        grenadePool = new ObjectPool(grenadePrefab, transform);

        InvokeRepeating("SpawnEnemies", 1.0f, 1.0f);
        InvokeRepeating("SpawnObstacles", 1.0f, 1.0f);
    }

    private void AddPool(GameObject prefab)
    {
        // Create new child component to hold pool objects
        GameObject child = new GameObject();
        child.name = prefab.name + " Pool"; 
        child.transform.parent = transform;

        objectPools.Add(prefab.name, new ObjectPool(prefab, child.transform));
    }

    void SpawnEnemies()
    {
        GameObject obj = objectPools["Enemy"].GetObject();
        obj.GetComponent<EnemyScript>().Spawn(new Vector3(38, -12), Quaternion.identity);
        obj.SetActive(true);
    }

    void SpawnObstacles()
    {
        GameObject obj = objectPools["Obstacle"].GetObject();
        obj.GetComponent<ObstacleScript>().Spawn(new Vector3(70, 15), Quaternion.identity);
        obj.SetActive(true);
    }
    
    
    
    public GameObject GetGrenade()
    {
        return grenadePool.GetObject();
    }

}
