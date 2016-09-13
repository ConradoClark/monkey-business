using UnityEngine;
using System.Collections;

[AddComponentMenu("Frosty-Movement/Conditions/FrostyPredicateAnd")]
public class FrostyPredicateAnd : FrostyMovementPredicate
{
    public override bool Value
    {
        get
        {
            for (int i = 0; i < predicates.Length; i++)
            {
                if (!predicates[i].Value) return false;
            }
            return true;
        }
    }

    public FrostyMovementPredicate[] predicates;
}
