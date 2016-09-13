using UnityEngine;
using System.Collections;

[AddComponentMenu("Frosty-Movement/Conditions/FrostyPredicateOr")]
public class FrostyPredicateOr : FrostyMovementPredicate
{
    public override bool Value
    {
        get
        {
            for (int i = 0; i < predicates.Length; i++)
            {
                if (predicates[i].Value) return true;
            }
            return false;
        }
    }

    public FrostyMovementPredicate[] predicates;
}
