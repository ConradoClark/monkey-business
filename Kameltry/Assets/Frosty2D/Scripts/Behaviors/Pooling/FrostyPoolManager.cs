using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FrostyPoolManager : MonoBehaviour
{
    public Dictionary<FrostyPoolInstance, FrostyPooledObject[]> pools { get; private set; }
    private GameObject pool;

    void Awake()
    {
        pools = new Dictionary<FrostyPoolInstance, FrostyPooledObject[]>();
        pool = new GameObject("pooling");
        pool.transform.SetParent(this.transform);
    }

    public void AddInstanceToPool(FrostyPoolInstance instance)
    {
        FrostyPoolInstance existing = this.pools.Keys.Where(p => p.objectType.GetInstanceID() == instance.objectType.GetInstanceID()).FirstOrDefault();
        this.pools.Add(instance, new FrostyPooledObject[instance.count]);
        for (int i = 0; i < this.pools[instance].Length; i++)
        {
            FrostyPoolableObject[] poolBehaviours;
            FrostyPooledObject[] existingPool = Object.ReferenceEquals(existing,null) ? new FrostyPooledObject[0] : pools.First(p => p.Key == existing).Value;

            if (!Object.ReferenceEquals(existing, null) && existingPool.Length > i)
            {
                this.pools[instance][i] = existingPool[i];
                poolBehaviours = this.pools[instance][i].poolObject.GetComponentsInChildren<FrostyPoolableObject>();
                this.pools[instance][i].poolBehaviours = poolBehaviours;
                for (int p = 0; p < poolBehaviours.Length; p++)
                {
                    poolBehaviours[p].Connect(instance);
                }
                continue;
            }

            this.pools[instance][i] = new FrostyPooledObject() { available = true };
            this.pools[instance][i].poolObject = GameObject.Instantiate(instance.objectType).transform;
            this.pools[instance][i].poolObject.gameObject.SetActive(false);
            this.pools[instance][i].poolObject.transform.SetParent(this.pool.transform, false);
            poolBehaviours = this.pools[instance][i].poolObject.GetComponentsInChildren<FrostyPoolableObject>();
            this.pools[instance][i].poolBehaviours = poolBehaviours;
            for (int p = 0; p < poolBehaviours.Length; p++)
            {
                poolBehaviours[p].Connect(instance);
            }
        }

        if (existing != null)
        {
            this.pools.Remove(existing);
        }
    }

    public void Cleanup()
    {
        pools = new Dictionary<FrostyPoolInstance, FrostyPooledObject[]>(pools.Where(p => p.Key!=null).ToDictionary(p => p.Key, p => p.Value));
    }

    public GameObject Retrieve(FrostyPoolInstance poolInstance)
    {
        return Retrieve(poolInstance, Vector3.zero, Quaternion.identity);
    }

    public GameObject Retrieve(FrostyPoolInstance poolInstance, Vector3 position)
    {
        return Retrieve(poolInstance, position, Quaternion.identity);
    }

    public GameObject Retrieve(FrostyPoolableObject pool)
    {
        return Retrieve(pool, Vector3.zero, Quaternion.identity);
    }

    public GameObject Retrieve(FrostyPoolableObject pool, Vector3 position)
    {
        return Retrieve(pool, position, Quaternion.identity);
    }

    public GameObject Retrieve(FrostyPoolableObject pool, Vector3 position, Quaternion rotation)
    {
        GameObject obj = null;

        if (pool == null || pool.poolInstance == null)
        {
            throw new System.Exception("Invalid pool Object");
        }

        // No pool? 
        if (!pools.ContainsKey(pool.poolInstance))
        {
            bool found = false;
            // Check if a similar pool exists and reconnect
            for (int i = 0; i < pools.Keys.Count; i++)
            {
                var elem = pools.Keys.ElementAt(i);
                if (elem.objectType.GetInstanceID() == pool.poolInstance.objectType.GetInstanceID())
                {
                    pool.Connect(pools.Keys.ElementAt(i), true);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // If not, use Unity's built-in instantiate
                obj = Instantiate(pool.poolInstance.objectType);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
        }
        return Retrieve(pool.poolInstance, position, rotation);
    }

    public GameObject Retrieve(FrostyPoolInstance pool, Vector3 position, Quaternion rotation)
    {
        GameObject obj = null;

        if (pool == null)
        {
            throw new System.Exception("Invalid or disconnected pool instance");
        }

        // No pool? 
        if (!pools.ContainsKey(pool))
        {
            obj = Instantiate(pool.objectType);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        // Try to find an available slot from pool
        for (int i = 0; i < pools[pool].Length; i++)
        {
            if (pools[pool][i].available)
            {
                pools[pool][i].poolObject.gameObject.SetActive(true);
                for (int p = 0; p < pools[pool][i].poolBehaviours.Length; p++)
                {
                    pools[pool][i].poolBehaviours[p].ResetState();
                }
                pools[pool][i].poolObject.position = position;
                pools[pool][i].poolObject.rotation = rotation;
                pools[pool][i].available = false;
                return pools[pool][i].poolObject.gameObject;
            }
        }

        // No room for pool, instantiate
        obj = Instantiate(pool.objectType);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void Release(FrostyPoolableObject pool, GameObject obj)
    {
        // No pool? Destroy object
        if (pool == null || pool.poolInstance == null)
        {
            GameObject.Destroy(obj);
            return;
        }

        // Unknown pool? Get out
        if (!pools.ContainsKey(pool.poolInstance))
        {
            bool found = false;
            // Check if a similar pool exists and reconnect
            for (int i = 0; i < pools.Keys.Count; i++)
            {
                if (pools.Keys.ElementAt(i).objectType.GetInstanceID() == obj.GetInstanceID())
                {
                    pool.Connect(pools.Keys.ElementAt(i), true);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // If not, get out
                return;
            }
        }

        Release(pool.poolInstance, obj);
    }

    public void Release(FrostyPoolInstance poolInstance, GameObject obj)
    {
        // No pool instance? Destroy object
        if (poolInstance == null)
        {
            GameObject.Destroy(obj);
            return;
        }

        // Unknown pool? Get out
        if (!pools.ContainsKey(poolInstance))
        {
            return;
        }

        // Try to find from pool and deactivate it
        for (int i = 0; i < pools[poolInstance].Length; i++)
        {
            if (pools[poolInstance][i].poolObject.transform.GetInstanceID() == obj.transform.GetInstanceID())
            {
                pools[poolInstance][i].available = true;
                pools[poolInstance][i].poolObject.gameObject.SetActive(false);
                pools[poolInstance][i].poolObject.transform.SetParent(pool.transform);
                return;
            }
        }

        // No pool, kill the object
        GameObject.Destroy(obj);
    }
}
