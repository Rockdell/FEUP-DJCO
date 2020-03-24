using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private int poolSize;
    private List<GameObject> pool;

    private GameObject prefab;
    private Transform holder;

    public ObjectPool(GameObject prefab, Transform holder, int poolSize)
    {
        pool = new List<GameObject>();
        (this.prefab, this.holder, this.poolSize) = (prefab, holder, poolSize);
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

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Object.Instantiate(prefab, holder);
            obj.SetActive(false);
            pool.Add(obj);
        }

        return GetObject();
    }
}
