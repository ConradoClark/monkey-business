using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class FrostyTime : MonoBehaviour
{
    protected Dictionary<TimeLayers, float> timeMultipliers;
    protected Dictionary<TimeLayers, float> timeElapsedPerLayer;
    protected Dictionary<string, FrostyAccumulatedTimeLayer> customTimeElapsed;

    public class FrostyAccumulatedTimeLayer
    {
        public TimeLayers layer;
        public float elapsedTime;
        public bool active = true;
    }

    void Awake()
    {
        timeMultipliers = new Dictionary<TimeLayers, float>();
        timeElapsedPerLayer = new Dictionary<TimeLayers, float>();
        customTimeElapsed = new Dictionary<string, FrostyAccumulatedTimeLayer>();
        var values = Enum.GetValues(typeof(TimeLayers)).OfType<TimeLayers>().ToArray();
        for (int i = 0; i < values.Length; i++)
        {
            timeElapsedPerLayer[values[i]] = 0f;
        }
    }

    void Update()
    {
        foreach(TimeLayers layer in timeElapsedPerLayer.Keys.ToArray())
        {
            timeElapsedPerLayer[layer] += GetDeltaTime(layer);
        }

        foreach(string counter in customTimeElapsed.Keys)
        {
            if (!customTimeElapsed[counter].active) return;
            customTimeElapsed[counter].elapsedTime += GetDeltaTime(customTimeElapsed[counter].layer);
        }
    }

    public void AddCustomTimeCounter(string timeCounter, TimeLayers layer)
    {
        if (customTimeElapsed.ContainsKey(timeCounter)) return;
        customTimeElapsed[timeCounter] = new FrostyAccumulatedTimeLayer() { layer = layer, elapsedTime = 0f };
    }

    public void PauseCustomTimeCounter(string timeCounter)
    {
        if (!customTimeElapsed.ContainsKey(timeCounter)) return;
        customTimeElapsed[timeCounter].active = false;
    }

    public void UnpauseCustomTimeCounter(string timeCounter)
    {
        if (!customTimeElapsed.ContainsKey(timeCounter)) return;
        customTimeElapsed[timeCounter].active = true;
    }

    public void ResetCustomTimeCounter(string timeCounter)
    {
        if (!customTimeElapsed.ContainsKey(timeCounter))
        {
            return;
        }
        customTimeElapsed[timeCounter].elapsedTime = 0f;
    }

    public float GetTotalElapsedTime(string timeCounter)
    {
        if (!customTimeElapsed.ContainsKey(timeCounter))
        {
            return 0f;
        }
        return customTimeElapsed[timeCounter].elapsedTime;
    }

    public float GetTotalElapsedTime(TimeLayers layer)
    {
        if (!timeElapsedPerLayer.ContainsKey(layer))
        {
            timeElapsedPerLayer[layer] = 0f;
        }
        return timeElapsedPerLayer[layer];
    }

    public float GetDeltaTime(TimeLayers layer)
    {
        if (!timeMultipliers.ContainsKey(layer))
        {
            timeMultipliers[layer] = 1.0f;
        }

        return Time.deltaTime * timeMultipliers[layer];
    }

    public float GetSmoothDeltaTime(TimeLayers layer)
    {
        if (!timeMultipliers.ContainsKey(layer))
        {
            timeMultipliers[layer] = 1.0f;
        }

        return Time.smoothDeltaTime * timeMultipliers[layer];
    }

    public float GetFixedDeltaTime(TimeLayers layer)
    {
        if (!timeMultipliers.ContainsKey(layer))
        {
            timeMultipliers[layer] = 1.0f;
        }

        return Time.fixedDeltaTime * timeMultipliers[layer];
    }

    public void SetLayerMultiplier(FrostyTimeLayerGroup group, float multiplier)
    {
        foreach (TimeLayers layer in group.children)
        {
            SetLayerMultiplier(layer, multiplier);
        }
    }

    public IEnumerator WaitForSeconds(TimeLayers layer, float seconds)
    {
        if (seconds <= 0f) yield break;
        while (seconds > 0f)
        {
            seconds -= this.GetFixedDeltaTime(layer);
            yield return 1;
        }
    }

    public void SetLayerMultiplier(TimeLayers layer, float multiplier)
    {
        timeMultipliers[layer] = multiplier;
    }

    public float GetLayerMultiplier(TimeLayers layer)
    {
        if (timeMultipliers.ContainsKey(layer))
        {
            return timeMultipliers[layer];
        }
        return 1.0f;
    }
}
