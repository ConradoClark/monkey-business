using UnityEngine;
using System.Collections;

public class FrostySlopeMovement : MonoBehaviour
{
    public FrostyKinematics kinematics;
    public Vector2 slopeDirection;
    public FrostyMovementPredicate isColliding;
    public FrostyEventOnCollision onCollisionEvent;
    public FrostyPatternMovement jumpMovement;
    public FrostyMovementPredicate onGround;
    private float speed_x;
    private float speed_y;
    private int direction;
    private Vector2 normal;
    private float timeElapsed;
    public float animSpeed { get; private set; }
    public TimeLayers timeLayer;

    private float speed_xdamp;

    void Start()
    {
        animSpeed = 1f;
    }

    void Update()
    {
        if (isColliding.Value && onCollisionEvent.impactOnPoints.Count > 0 && !jumpMovement.IsActive())
        {
            speed_x = speed_y = kinematics.GetSpeed(new Vector2(slopeDirection.x, 0));
            direction = speed_x > 0 ? 1 : -1;
            var slope = onCollisionEvent.impactOnPoints[0].collider.GetComponent<FrostySlope>();
            normal = slope != null ? slope.direction / 2 : Vector2.zero;
            timeElapsed = 0;
        }

        timeElapsed += Toolbox.Instance.time.GetSmoothDeltaTime(timeLayer);
        if (!jumpMovement.IsActive())
        {
            bool sameDir = slopeDirection.x * direction > 0;
            kinematics.ApplyMovement(new Vector2(0, slopeDirection.y * 5), (normal * speed_y).magnitude * direction * (direction == -1 ? 4 : 1));
            if (speed_x > 0)
            {
                animSpeed = direction == -1 ? 1.1f : 0.65f;
            }
        }
        else
        {
            if (direction == 1)
            {
                speed_x = 0;
            }
            speed_y = 0;
            animSpeed = 1f;
        }

        kinematics.ApplyMovement(new Vector2(slopeDirection.x / 2, 0), (normal * speed_x).magnitude * direction * (direction == -1 ? 4 : 1)/5f);
        speed_y = direction == -1 && timeElapsed < 0.3f ? speed_y : 0;
        if (speed_x >= 0)
        {
            speed_x = Mathf.Clamp(speed_x - Toolbox.Instance.time.GetSmoothDeltaTime(timeLayer), 0, float.MaxValue);
        }
        else
        {
            speed_x = Mathf.Clamp(speed_x + Toolbox.Instance.time.GetSmoothDeltaTime(timeLayer), float.MinValue, 0);
        }

        if (!isColliding.Value && onGround.Value)
        {
            speed_x=0f;
        }
    }
}
