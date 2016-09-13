using UnityEngine;
using System.Collections;

public class FrostyMovementReversePredicate : FrostyMovementPredicate
{
    public FrostyMovementPredicate predicate;
    public override bool Value { get { return !predicate.Value; } }
}
