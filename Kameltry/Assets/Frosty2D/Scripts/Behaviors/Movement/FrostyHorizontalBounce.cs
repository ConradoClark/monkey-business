using UnityEngine;
using System.Collections;

public class FrostyHorizontalBounce : MonoBehaviour
{

    public FrostyPatternMovement movement;
    public FrostyMovementPredicate predicate;
    public int direction;

    void Update()
    {
        if (predicate.Value)
        {
            movement.SetHorizontalAxisSign(direction);
        }
    }
}
