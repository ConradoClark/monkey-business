using UnityEngine;
using System.Collections;

public class FrostyPool : MonoBehaviour
{
    public FrostyPoolInstance[] pools;

    void Start()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            Toolbox.Instance.pool.AddInstanceToPool(pools[i]);
        }
    }
}
