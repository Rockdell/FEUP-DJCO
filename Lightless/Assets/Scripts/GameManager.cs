using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject grenadeHolder;
    public GameObject grenadePrefab;

    private const int GRENADE_POOL_SIZE = 10;
    private List<GameObject> grenadePool;
    
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        grenadePool = new List<GameObject>();
        for (int i = 0; i < GRENADE_POOL_SIZE; i++)
        {
            GameObject obj = Instantiate(grenadePrefab, grenadeHolder.transform);
            obj.SetActive(false);
            grenadePool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public GameObject GetGrenade()
    {
        for (int i = 0; i < grenadePool.Count; i++)
        {
            if (!grenadePool[i].activeInHierarchy)
                return grenadePool[i];
        }

        for (int i = 0; i < GRENADE_POOL_SIZE; i++)
        {
            GameObject obj = Instantiate(grenadePrefab, grenadeHolder.transform);
            obj.SetActive(false);
            grenadePool.Add(obj);
        }

        return GetGrenade();
    }
}
