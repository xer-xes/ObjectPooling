﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private static Dictionary<PooledMonoBehaviour, Pool> pools = new Dictionary<PooledMonoBehaviour, Pool>();

    private Queue<PooledMonoBehaviour> objects = new Queue<PooledMonoBehaviour>();
    private List<PooledMonoBehaviour> disabledObjects = new List<PooledMonoBehaviour>();

    private PooledMonoBehaviour prefab;
    public static Pool GetPool(PooledMonoBehaviour prefab)
    {
        if (pools.ContainsKey(prefab))
            return pools[prefab];

        var pool = new GameObject("Pool-" + (prefab as Component).name).AddComponent<Pool>();
        pool.prefab = prefab;

        pool.GrowPool();
        pools.Add(prefab, pool);
        return pool;
    }

    public T Get<T>() where T : PooledMonoBehaviour
    {
        if(objects.Count == 0)
        {
            GrowPool();
        }

        var pooledObject = objects.Dequeue();
        return pooledObject as T;
    }

    public void GrowPool()
    {
        for (int i = 0; i < prefab.InitialPoolSize; i++)
        {
            var pooledObject = Instantiate(this.prefab) as PooledMonoBehaviour;
            (pooledObject as Component).gameObject.name += $" {i}";

            pooledObject.OnDestroyEvent += () => AddObjectToAvailable(pooledObject);

            (pooledObject as Component).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        MakeDisabledObjectsChildren();
    }

    private void MakeDisabledObjectsChildren()
    {
        if(disabledObjects.Count > 0)
        {
            foreach(var pooledObject in disabledObjects)
            {
                if(pooledObject.gameObject.activeInHierarchy == false)
                {
                    (pooledObject as Component).transform.SetParent(transform);
                }
            }
            disabledObjects.Clear();
        }
    }

    private void AddObjectToAvailable(PooledMonoBehaviour pooledObject)
    {
        disabledObjects.Add(pooledObject);
        objects.Enqueue(pooledObject);
    }
}
