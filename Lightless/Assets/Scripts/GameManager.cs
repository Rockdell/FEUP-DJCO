using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Move somewhere else
    public GameObject grenadePrefab;
    public ObjectPool grenadePool;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        grenadePool = new ObjectPool(grenadePrefab, transform);
    }

    public GameObject GetGrenade()
    {
        return grenadePool.GetObject();
    }

}
