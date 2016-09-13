using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FrostyEventOnCollision : FrostyOnCollision
{
    public FrostyKinematics kinematics;
    public FrostyCollision[] colliders;
    public List<RaycastHit2D> impactOnPoints { get; private set; }
    public bool clamp = true;
    public bool useTriggerDistance = true;
    public float triggerDistance = 0.5f;
    public float expireAfter = 0f;
    private bool setForExpiration;
    private float expiration;
    public TimeLayers timeLayer;

    void Start()
    {
        impactOnPoints = new List<RaycastHit2D>();
        expiration = expireAfter;
    }

    void Update()
    {
        if (impactOnPoints == null) return;

        impactOnPoints.Clear();
        if (kinematics == null) return;

        bool value = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            FrostyCollision collision = colliders[i];
            if (collision.AllHits == null) continue;
            for (int j = 0; j < collision.AllHits.Length; j++)
            {
                RaycastHit2D hit = collision.AllHits[j];
                if (hit.collider != null)
                {
                    Vector2 validDirection = collision.GetClampDirection();
                    float distance = Vector2.Distance(Vector2.Scale(((Vector2)collision.transform.position + collision.offset),validDirection),
                                                      Vector2.Scale(hit.point,validDirection));
                    if (!useTriggerDistance || distance <= triggerDistance)
                    {
                        value = true;
                        impactOnPoints.Add(hit);
                        collision.Decay();
                    }
                    if (clamp)
                    {
                        kinematics.ClampPosition(validDirection, -validDirection.y * (transform.position.y + (validDirection * distance).y) + validDirection.x * (transform.position.x + (validDirection * distance).x));
                    }
                }
            }
        }

        if (PredicateOnCollision != null)
        {
            PredicateOnCollision.SetValue(value);
        }

        if (ExtraPredicatesOnCollision != null)
        {
            for (int i = 0; i < ExtraPredicatesOnCollision.Length; i++)
            {
                ExtraPredicatesOnCollision[i].SetValue(ExtraPredicatesOnCollision[i].Value || value);
            }
        }

        if (value && expireAfter>0 && !setForExpiration)
        {
            setForExpiration = true;
        }

        if (setForExpiration)
        {
            expiration -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
            if (expiration < 0) this.enabled = false;
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        this.enabled = true;
        this.setForExpiration = false;
        this.expiration = expireAfter;
    }
}
