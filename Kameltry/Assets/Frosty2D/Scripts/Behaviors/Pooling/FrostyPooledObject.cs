using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class FrostyPooledObject
{
    public Transform poolObject;
    public bool available;
    public FrostyPoolableObject[] poolBehaviours;
}
