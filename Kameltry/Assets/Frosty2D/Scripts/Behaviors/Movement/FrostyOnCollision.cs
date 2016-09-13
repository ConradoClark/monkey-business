using UnityEngine;
using System.Collections;

public class FrostyOnCollision : FrostyPoolableObject
{
    public FrostyMovementPredicateCustom PredicateOnCollision;
    public FrostyMovementPredicateCustom[] ExtraPredicatesOnCollision;
}
