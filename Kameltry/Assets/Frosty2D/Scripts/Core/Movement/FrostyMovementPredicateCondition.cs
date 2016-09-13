using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class FrostyMovementPredicateCondition : FrostyMovementPredicate
{
    public FrostyMovementPredicate predicate;
    public FrostyPatternMovement movement;
    public bool preventIfActive;
    public bool changeIfActivating;
    public bool changeIfOnLoop;
    public bool changeIfDeactivating;
    public float changeOnlyAfterSeconds;
    public float changeOnlyBeforeSeconds;
    public float preserveForSeconds;
    bool? preservedValue;
    Stack<bool> startedCoroutines = new Stack<bool>();
    public TimeLayers timeLayer;

    public bool debug;

    //void OnGUI()
    //{
    //    if (debug)
    //    {
    //        GUI.Label(new Rect(10,10,1000,100),this.Value.ToString());
    //    }
    //}

    public override bool Value
    {
        get
        {
            if (predicate.Value)
            {
                StartCoroutine(PreserveValue());
            }

            if (movement != null)
            {
                bool isActivating = movement.IsActivating;
                bool isOnLoop = movement.IsOnLoop;
                bool isDeactivating = movement.IsDeactivating;

                if (preventIfActive && (movement.IsActive() && !isDeactivating)) return false;
                if (changeIfActivating && !isActivating) return false;
                if (changeIfOnLoop && !isOnLoop) return false;
                if (changeIfDeactivating && !isDeactivating) return false;

                if (changeOnlyAfterSeconds > 0 && !isDeactivating && movement.GetCurrentTime() < changeOnlyAfterSeconds) return false;
                if (changeOnlyBeforeSeconds > 0 && !isDeactivating && movement.GetCurrentTime() > changeOnlyBeforeSeconds) return false;
            }

            if (debug && (preservedValue ?? predicate.Value))
            {
               
            }
            return preservedValue ?? predicate.Value;
        }
    }

    IEnumerator PreserveValue()
    {
        if (!predicate.Value) yield break;
        bool valueToPreserve = predicate.Value;
        
        if (preservedValue != null && valueToPreserve != preservedValue)
        {
            while (startedCoroutines.Count > 0)
            {
                yield return 1;
            }
        }

        preservedValue = valueToPreserve = predicate.Value;
        startedCoroutines.Push(valueToPreserve);
        yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, preserveForSeconds);

        startedCoroutines.Pop();
        preservedValue = startedCoroutines.Count == 0 ? null : preservedValue;
    }
}
