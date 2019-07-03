using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PoolPreparer : MonoBehaviour
{
    [SerializeField]
    public PooledMonoBehaviour[] prefabs;

    [SerializeField]
    private int initialPoolSize = 100;

    private void Awake()
    {
        foreach (var prefab in prefabs)
        {
            if(prefab == null)
            {
                Debug.LogError("Null Prefab in PoolPreparer");
            }
            else
            {
                PooledMonoBehaviour poolablePrefab = prefab.GetComponent<PooledMonoBehaviour>();
                if(poolablePrefab == null)
                {
                    Debug.LogError("Prefab Does not Contain an IPoolable and Can't be Pooled");
                }
                else
                {
                    Pool.GetPool(poolablePrefab).GrowPool();
                }
            }
        }
    }

    private void OnValidate()
    {
        List<GameObject> prefabsToRemove = new List<GameObject>();
        foreach (var prefab in prefabs.Where(t => t != null))
        {
            if(!PrefabUtility.IsPartOfPrefabAsset(prefab))
            {
                Debug.LogError(string.Format("{0} is not a Prefab. It Has Been Removed.", prefab.gameObject.name));
                prefabsToRemove.Add(prefab.gameObject);
            }

            PooledMonoBehaviour poolablePrefab = prefab.GetComponent<PooledMonoBehaviour>();
            if(poolablePrefab == null)
            {
                Debug.LogError("Prefab Does not Contain an IPoolable and Can't be Pooled. It Has Been Removed");
                prefabsToRemove.Add(prefab.gameObject);
            }
        }

        prefabs = prefabs.Where(t => t != null && prefabsToRemove.Contains(t.gameObject) == false).ToArray();
    }
}
