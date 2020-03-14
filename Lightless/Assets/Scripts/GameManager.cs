using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum ObjectType
    {
        LightBullet, Grenade, Enemy, Obstacle
    };

    public static GameManager Instance;
    public GameObject lightBulletPrefab;
    public GameObject grenadePrefab;
    public GameObject enemyPrefab;
    public GameObject obstaclePrefab;
    private Dictionary<ObjectType, ObjectPool> _objectPools;

    void Awake()
    {
        Instance = this;
        
        // Initialize object pools
        _objectPools = new Dictionary<ObjectType, ObjectPool>();
        AddPool(ObjectType.LightBullet, lightBulletPrefab);
        AddPool(ObjectType.Grenade, grenadePrefab);
        AddPool(ObjectType.Enemy, enemyPrefab);
        AddPool(ObjectType.Obstacle, obstaclePrefab);
    }

    void Start()
    {
        InvokeRepeating("SpawnEnemies", 1.0f, 1.0f);
        InvokeRepeating("SpawnObstacles", 1.0f, 1.0f);
    }

    void AddPool(ObjectType type, GameObject prefab)
    {
        // Create new child component to hold pool objects
        GameObject child = new GameObject(type + " Pool");
        child.transform.parent = transform;

        _objectPools.Add(type, new ObjectPool(prefab, child.transform));
    }

    public GameObject GetObject(ObjectType type)
    {
        return _objectPools[type].GetObject();
    }
    
    // Spawners
    
    void SpawnEnemies()
    {
        GameObject obj = GetObject(ObjectType.Enemy);
        obj.GetComponent<EnemyScript>().Spawn(new Vector3(38, -12), Quaternion.identity);
        obj.SetActive(true);
    }

    void SpawnObstacles()
    {
        GameObject obj =  GetObject(ObjectType.Obstacle);
        obj.GetComponent<ObstacleScript>().Spawn(new Vector3(70, 15), Quaternion.identity);
        obj.SetActive(true);
    }

}
