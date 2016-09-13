using UnityEngine;
using System.Linq;
using System.Collections;
using Assets.Frosty2D.Scripts.Core.Movement;

[AddComponentMenu("Frosty-Movement/Pattern Movement")]
public class FrostyPatternMovement : MonoBehaviour
{
    [Header("Kinematics Reference")]
    public FrostyKinematics kinematics;

    [Header("Movement Pattern")]
    public FrostySingleMovementPattern[] patterns;

    [Header("Conditions")]
    public FrostyMovementPredicate[] resetOnPredicate;
    public FrostyMovementPredicate[] reactivateOnPredicate;
    public FrostyMovementPredicate[] deactivateOnPredicate;
    public FrostyMovementPredicate[] abortOnPredicate;

    [Header("Modifiers")]
    public FrostyMovementDampener[] movementDampeners;

    private Vector2 currentDirection;
    private float currentSpeed;
    private Vector2 rawMovement;
    private float currentDamp = 1;
    private float currentDampSpeed = 0;

    public TimeLayers timeLayer;

    void Update()
    {
        currentSpeed = 0f;
        rawMovement = Vector2.zero;
        currentDirection = Vector2.zero;

        if (resetOnPredicate.Any(pred => pred.Value))
        {
            Reactivate(false, true);
        }

        if (reactivateOnPredicate.Any(pred => pred.Value))
        {
            Reactivate(false, false);
        }

        if (deactivateOnPredicate.Any(pred => pred.Value))
        {
            this.Deactivate();
        }

        bool active = this.IsActive();
        if (abortOnPredicate.Any(pred => pred.Value) && active)
        {
            this.Abort();
        }

        if (!active)
        {
            return;
        }

        var damp = movementDampeners.Where(d => d.predicates.All(p => p.Value)).ToArray();
        float dampAmount = damp.Aggregate<FrostyMovementDampener, float>(1, (val, d) => val * d.dampAmount);
        dampAmount = Mathf.SmoothDamp(currentDamp, dampAmount, ref currentDampSpeed, 0.15f);
        currentDamp = dampAmount;

        if (patterns == null) return;

        for (int i = 0; i < patterns.Length; i++)
        {
            FrostySingleMovementPattern pattern = patterns[i];
            float speed;
            Vector2 dir = pattern.Evaluate(Toolbox.Instance.time.GetFixedDeltaTime(timeLayer), out speed);
            rawMovement += (dir.normalized * speed) * dampAmount;
        }

        currentDirection = rawMovement.normalized;
        currentSpeed = rawMovement.magnitude;

        if (kinematics != null)
        {
            kinematics.ApplyMovement(currentDirection, currentSpeed);
        }
    }

    public float GetCurrentTime()
    {
        return patterns.Select(p=>p.GetCurrentTime()).DefaultIfEmpty(0).Sum();
    }

    public Vector2 GetDirection()
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < patterns.Length; i++)
        {
            FrostySingleMovementPattern pattern = patterns[i];
            sum += pattern.direction;
        }
        return sum.normalized;
    }

    public void SetHorizontalAxisSign(int sign)
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < patterns.Length; i++)
        {
            FrostySingleMovementPattern pattern = patterns[i];
            pattern.direction = new Vector2(Mathf.Sign(sign) * Mathf.Sign(pattern.direction.x) * pattern.direction.x, pattern.direction.y);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < patterns.Length; i++)
        {
            FrostySingleMovementPattern pattern = patterns[i];
            pattern.direction = direction;
        }
    }

    public bool IsActive()
    {
        return patterns.Any(p => p.active);
    }

    public bool IsActivating
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].currentState == FrostySingleMovementPattern.STATE_ACTIVATION)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool IsOnLoop
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].currentState != FrostySingleMovementPattern.STATE_LOOP)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool IsDeactivating
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].currentState == FrostySingleMovementPattern.STATE_DEACTIVATION)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool HasFinished
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].active)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void Reactivate(bool keepSpeed = true, bool evenIfActive=true)
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].Reactivate(keepSpeed,evenIfActive);
        }
    }

    public void Deactivate()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].Deactivate();
        }
    }

    public void Abort()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].Deactivate();
            patterns[i].active = false;
        }
    }
}
