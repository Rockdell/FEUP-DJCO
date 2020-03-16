using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum ObjectType
    {
        LightBullet, Grenade, Zombie, ZombieBullet,
        Firefly, DropLight
    };

    public static GameManager Instance { get; private set; }

    public Camera gameCamera { get; private set; }
    public Vector2 screenBounds { get; private set; }

    public GameObject lightBulletPrefab;
    public GameObject grenadePrefab;
    public GameObject zombiePrefab;
    public GameObject zombieBulletPrefab;
    public GameObject fireflyPrefab;


    //public GameObject grenadeFirePrefab;
    //public GameObject enemyPrefab;
    //public GameObject obstaclePrefab;
    public GameObject dropLightPrefab;

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
        AddPool(ObjectType.Zombie, zombiePrefab);
        AddPool(ObjectType.ZombieBullet, zombieBulletPrefab);
        AddPool(ObjectType.Firefly, fireflyPrefab);
        //AddPool(ObjectType.GrenadeFire, grenadeFirePrefab);
        //AddPool(ObjectType.Enemy, enemyPrefab);
        //AddPool(ObjectType.Obstacle, obstaclePrefab);
        AddPool(ObjectType.DropLight, dropLightPrefab);
    }

    void Start()
    {
        StartCoroutine("SpawnWaves");
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

    public GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator SpawnWaves()
    {
        IWave currentWave = new WaveII();

        while (true)
        {
            if (!currentWave.isOver())
                yield return new WaitForSeconds(2.5f);
            else
                currentWave = new WaveII();
        }
    }
}
