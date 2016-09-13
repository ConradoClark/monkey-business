using System;
using UnityEngine;

[AddComponentMenu("")]
public class FrostyMovementDampener : MonoBehaviour
{
    public FrostyMovementPredicate[] predicates;
    public bool inverse;
    public float dampAmount;
}
