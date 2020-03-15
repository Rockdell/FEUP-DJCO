using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum ObjectType
    {
        LightBullet, Grenade, GrenadeFire, Enemy, Obstacle, Firefly
    };

    public static GameManager Instance { get; private set; }

    public Camera gameCamera { get; private set; }
    public Vector2 screenBounds { get; private set; }

    public GameObject lightBulletPrefab;
    public GameObject grenadePrefab;
    public GameObject grenadeFirePrefab;
    public GameObject enemyPrefab;
    public GameObject obstaclePrefab;
    public GameObject fireflyPrefab;
    private Dictionary<ObjectType, ObjectPool> _objectPools;

    void Awake()
    {
        Instance = this;

        gameCamera = Camera.main;
        screenBounds = gameCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Initialize object pools
        _objectPools = new Dictionary<ObjectType, ObjectPool>();
        AddPool(ObjectType.LightBullet, lightBulletPrefab);
        AddPool(ObjectType.Grenade, grenadePrefab);
        AddPool(ObjectType.GrenadeFire, grenadeFirePrefab);
        AddPool(ObjectType.Enemy, enemyPrefab);
        AddPool(ObjectType.Obstacle, obstaclePrefab);
        AddPool(ObjectType.Firefly, fireflyPrefab);
    }

    void Start()
    {
        //InvokeRepeating("SpawnEnemies", 1.0f, 1.0f);
        InvokeRepeating("SpawnObstacles", 1.0f, 2.0f);
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
