using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostyRandom : MonoBehaviour
{

    private Dictionary<string, System.Random> generators;

    void Awake()
    {
        generators = new Dictionary<string, System.Random>();
    }

    public void AddGenerator(string generatorName, int generatorSeed)
    {
        if (generators.ContainsKey(generatorName)) return;
        generators[generatorName] = new System.Random(generatorSeed);
    }

    public float GetRange(string generator, float minRangeInclusive, float maxRangeInclusive)
    {
        if (!generators.ContainsKey(generator))
        {
            generators[generator] = new System.Random();
        }
        return RescaleDouble(generators[generator].NextDouble()) * (maxRangeInclusive - minRangeInclusive) + minRangeInclusive;
    }

    public int GetRange(string generator, int minRangeInclusive, int maxRangeExclusive)
    {
        if (!generators.ContainsKey(generator))
        {
            generators[generator] = new System.Random();
        }

        return generators[generator].Next(minRangeInclusive, maxRangeExclusive);
    }

    public bool GetBool(string generator)
    {
        if (!generators.ContainsKey(generator))
        {
            generators[generator] = new System.Random();
        }

        return generators[generator].Next(0, 2) == 0;
    }

    private float RescaleDouble(double nextDouble)
    {
        return (float)nextDouble / 0.99f;
    }
}
