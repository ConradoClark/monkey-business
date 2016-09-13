using System;
using UnityEngine;

namespace Assets.Frosty2D.Scripts.Core.Movement
{
    [Serializable]
    public class FrostyMovementControllerInput
    {
        [Header("Movement Reference")]
        public FrostyPatternMovement movement;

        [Header("Conditions")]
        public FrostyMovementPredicate[] conditions;

        [Header("Input")]
        public FrostyInputActionFragment action;
        public FrostyMovementOptions behavior;
        public bool toggle;
        public bool exclusive;
        public bool deactivateOnRelease;
        public bool repeatOnHold;
        public bool keepSpeed;
        public int priority;
        public bool repeatWhenInactive;  
    }
}
