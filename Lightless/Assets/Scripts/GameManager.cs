using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum ObjectType
    {
        LightBullet, Grenade, Zombie, ZombieBullet,
        Firefly, RedLight, Boss, DarkBullet, 
        Burn, ForceField, DropLight
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
    public GameObject bossPrefab;
    public GameObject darkBulletPrefab;
    public GameObject burnPrefab;
    public GameObject forceFieldPrefab;
    public GameObject dropLightPrefab;

    private Dictionary<ObjectType, ObjectPool> _objectPools;

    void Awake()
    {
        Instance = this;

        gameCamera = Camera.main;
        screenBounds = gameCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Initialize object pools
        _objectPools = new Dictionary<ObjectType, ObjectPool>();
        AddPool(ObjectType.LightBullet, lightBulletPrefab, 10);
        AddPool(ObjectType.Grenade, grenadePrefab, 5);
        AddPool(ObjectType.Zombie, zombiePrefab, 5);
        AddPool(ObjectType.ZombieBullet, zombieBulletPrefab, 10);
        AddPool(ObjectType.Firefly, fireflyPrefab, 5);
        AddPool(ObjectType.RedLight, redLightPrefab, 3);
        AddPool(ObjectType.Boss, bossPrefab, 1);
        AddPool(ObjectType.DarkBullet, darkBulletPrefab, 10);
        AddPool(ObjectType.Burn, burnPrefab, 3);
        AddPool(ObjectType.ForceField, forceFieldPrefab, 5);
        AddPool(ObjectType.DropLight, dropLightPrefab, 3);
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    void AddPool(ObjectType type, GameObject prefab, int size)
    {
        // Create new child component to hold pool objects
        GameObject child = new GameObject(type + " Pool");
        child.transform.parent = transform;

        _objectPools.Add(type, new ObjectPool(prefab, child.transform, size));
    }

    public void CleanPools()
    {
        foreach (var entry in _objectPools)
        {
            entry.Value.Clean();
        }
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
            activeWaves.Add(new ZombieWave(3, true, true, true));
        });

        // Wave II
        waves.Enqueue(() =>
        {
            activeWaves.Add(new FireflyWave(25, true, true, true));
        });

        // Wave III
        waves.Enqueue(() =>
        {
            activeWaves.Add(new ZombieWave(3, true, true, true));
            activeWaves.Add(new FireflyWave(25, false, false, false));
        });

        // Wave IV
        waves.Enqueue(() =>
        {
            activeWaves.Add(new BossWave(1, false, true, true));
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
                yield return new WaitForSeconds(1.5f);

                waves.Dequeue()();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
