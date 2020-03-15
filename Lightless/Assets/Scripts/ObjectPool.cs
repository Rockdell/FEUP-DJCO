using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private const int DEFAULT_POOL_SIZE = 10;
    private List<GameObject> pool;

    private GameObject prefab;
    private Transform holder;

    public ObjectPool(GameObject prefab, Transform holder)
    {
        pool = new List<GameObject>();
        (this.prefab, this.holder) = (prefab, holder);
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        for (int i = 0; i < DEFAULT_POOL_SIZE; i++)
        {
            GameObject obj = Object.Instantiate(prefab, holder);
            obj.SetActive(false);
            pool.Add(obj);
        }

        return GetObject();
    }
}
