using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Frosty2D.Scripts.Core.Movement
{
    [Serializable]
    public class FrostySingleMovementPattern
    {
        public const int STATE_ACTIVATION = 0;
        public const int STATE_LOOP = 1;
        public const int STATE_DEACTIVATION = 2;

        [Header("Movement Pattern")]
        public AnimationCurve activation;
        public AnimationCurve loop;
        public AnimationCurve deactivation;
        public Vector2 direction;

        [Header("Loop")]
        public int timesToRepeat;
        public bool loopIndefinitely;

        [Header("Velocity")]
        public float minSpeed;
        public float maxSpeed;
        private float speedWhenChanged;
        private float currentSpeed;

        [Header("State")]
        public int currentState;
        public bool active;

        private float currentMinSpeed;
        private float currentTime;
        private int repeat;

        public FrostySingleMovementPattern()
        {
            this.currentMinSpeed = minSpeed;
        }

        public Vector2 Evaluate(float deltaTime, out float speed)
        {
            if (!this.active || 
                (currentTime > deactivation.keys[deactivation.keys.Length - 1].time && currentState == FrostySingleMovementPattern.STATE_DEACTIVATION))
            {
                speed = 0f;
                this.active = false;
                return Vector2.zero;
            }

            if (currentTime > activation.keys[activation.keys.Length - 1].time && currentState == FrostySingleMovementPattern.STATE_ACTIVATION)
            {
                this.ChangeState(FrostySingleMovementPattern.STATE_LOOP);
            }

            if (currentTime > loop.keys[loop.keys.Length-1].time && currentState == FrostySingleMovementPattern.STATE_LOOP)
            {
                repeat++;
                if (!loopIndefinitely && repeat >= timesToRepeat)
                {
                    this.ChangeState(FrostySingleMovementPattern.STATE_DEACTIVATION);
                }
            }

            AnimationCurve[] curves = new[] { activation, loop, deactivation };
            AnimationCurve currentCurve = curves[currentState % curves.Length];

            float calculatedMaxSpeed = (currentState == STATE_DEACTIVATION ? speedWhenChanged : maxSpeed);
            float calculatedMinSpeed = (currentState == STATE_ACTIVATION ? this.currentMinSpeed : minSpeed);

            float curveResult = currentCurve.Evaluate(currentTime);
            speed = currentSpeed = (calculatedMinSpeed + curveResult * (calculatedMaxSpeed - calculatedMinSpeed)) * deltaTime*60;
            currentTime += deltaTime;
            return direction;
        }

        public void Reactivate(bool keepSpeed = true, bool evenIfActive=true)
        {
            if (active && !evenIfActive) return;
            this.active = true;
            this.ChangeState(FrostySingleMovementPattern.STATE_ACTIVATION);
            this.currentMinSpeed = keepSpeed ? this.speedWhenChanged : minSpeed;
        }

        public void Deactivate()
        {
            this.ChangeState(FrostySingleMovementPattern.STATE_DEACTIVATION);
        }

        public void ChangeState(int newState)
        {
            this.currentTime = 0f;
            this.currentState = newState;
            this.speedWhenChanged = currentSpeed;
        }

        public float GetCurrentTime()
        {
            return currentTime;
        }
    }
}
