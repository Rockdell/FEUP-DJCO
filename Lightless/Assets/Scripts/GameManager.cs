using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum ObjectType
    {
        LightBullet, Grenade, Zombie, ZombieBullet,
        Firefly, RedLight, DropLight
    };

    public static GameManager Instance { get; private set; }

    public Camera gameCamera { get; private set; }
    public Vector2 screenBounds { get; private set; }

    public GameObject lightBulletPrefab;
    public GameObject grenadePrefab;
    public GameObject zombiePrefab;
    public GameObject zombieBulletPrefab;
    public GameObject fireflyPrefab;
    public GameObject redLightPrefab;

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
        AddPool(ObjectType.RedLight, redLightPrefab);
        //AddPool(ObjectType.GrenadeFire, grenadeFirePrefab);
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
        List<IWave> activeWaves = new List<IWave>();

        Queue<Action> waves = new Queue<Action>();

        // Wave I
        waves.Enqueue(() =>
        {
            activeWaves.Add(new ZombieWave(3, true));
        });

        // Wave II
        waves.Enqueue(() =>
        {
            activeWaves.Add(new FireflyWave(25, true));
        });

        // Wave III
        waves.Enqueue(() =>
        {
            activeWaves.Add(new ZombieWave(3, true));
            activeWaves.Add(new FireflyWave(25));
        });

        waves.Dequeue()();

        while (waves.Count > 0)
        {
            foreach (var wave in activeWaves.ToArray())
            {
                if (wave.isOver)
                    activeWaves.Remove(wave);
            }

            if (activeWaves.Count == 0)
            {
                yield return new WaitForSeconds(2.5f);

                waves.Dequeue()();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }


        //Queue<Lazy<IWave>> waves = new Queue<Lazy<IWave>>();

        //// WaveI, WaveII, WaveIII
        //waves.Enqueue(new Lazy<IWave>(() => { return new ZombieWave(3); }));
        //waves.Enqueue(new Lazy<IWave>(() => { return new FireflyWave(25); }));
        //waves.Enqueue(new Lazy<IWave>(() => { return new ZombieFireflyWave(); }));

        //IWave currentWave = waves.Dequeue().Value;

        //while (waves.Count > 0)
        //{
        //    if (!currentWave.isOver)
        //    {
        //        yield return new WaitForSeconds(1.0f);
        //    }
        //    else
        //    {
        //        yield return new WaitForSeconds(2.5f);
        //        currentWave = waves.Dequeue().Value;
        //    }
        //}
    }
}
