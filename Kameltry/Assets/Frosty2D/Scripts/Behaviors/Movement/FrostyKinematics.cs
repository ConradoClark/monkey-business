using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Frosty-Movement/Kinematics")]
public class FrostyKinematics : MonoBehaviour
{
    private List<Vector2> forces;
    private const int CLAMP_UP = 0;
    private const int CLAMP_RIGHT = 1;
    private const int CLAMP_DOWN = 2;
    private const int CLAMP_LEFT = 3;
    private float[] clamp;
    public float pauseTime;
    private float MAXIMUM_PAUSE_TIME = 10f;
    private TimeLayers timeLayer;
    private bool init = false;

    private Vector2[] intentions;

    public void PauseKinematics(float duration)
    {
        if (!init) return;
        this.pauseTime = duration;
    }

    void Start()
    {
        init = true;
        forces = new List<Vector2>();
        clamp = new float[4];
        this.ResetMovement();
    }

    void Update()
    {
        pauseTime = Mathf.Clamp(pauseTime - Toolbox.Instance.time.GetDeltaTime(timeLayer), 0, MAXIMUM_PAUSE_TIME);
        if (pauseTime>0f) return;

        Move();
    }

    void LateUpdate()
    {
        ResetMovement();
    }

    public float GetSpeed(Vector2 direction)
    {
        if (!init) return 0f;

        if (forces == null) forces = new List<Vector2>();
        direction.Normalize();
        return Vector2.Dot(direction, forces.Any() ? forces.Aggregate((v1, v2) => v1 + v2) : Vector2.zero);
    }

    public float GetSpeedIntention(Vector2 direction)
    {
        if (intentions == null) return 0f;
        direction.Normalize();
        return Vector2.Dot(direction, intentions.Any() ? intentions.Aggregate((v1, v2) => v1 + v2) : Vector2.zero);
    }

    private void Move()
    {
        intentions = forces.ToArray();
        Vector3 allForces = forces.Any() ? forces.Aggregate((v1, v2) => v1 + v2) : Vector2.zero;
        allForces += this.transform.position;
        float clampX = Mathf.Clamp(allForces.x, float.IsNaN(clamp[CLAMP_LEFT]) ? allForces.x : clamp[CLAMP_LEFT],
                                                float.IsNaN(clamp[CLAMP_RIGHT]) ? allForces.x : clamp[CLAMP_RIGHT]);

        float clampY = Mathf.Clamp(allForces.y, float.IsNaN(clamp[CLAMP_DOWN]) ? allForces.y : clamp[CLAMP_DOWN],
                                                float.IsNaN(clamp[CLAMP_UP]) ? allForces.y : clamp[CLAMP_UP]);

        allForces = new Vector3(clampX, clampY);
        this.transform.position = new Vector3(allForces.x, allForces.y, this.transform.position.z);
    }

    private void ResetMovement()
    {
        forces.Clear();
        clamp[CLAMP_UP] = clamp[CLAMP_RIGHT] = clamp[CLAMP_DOWN] = clamp[CLAMP_LEFT] = float.NaN;
    }

    public void ApplyMovement(Vector2 direction, float speed)
    {
        if (!init) return;
        forces.Add(direction.normalized * speed);
    }

    public void ClampPosition(Vector2 direction, float value)
    {
        if (!init) return;
        Vector2 point = direction.normalized * value;

        clamp[CLAMP_RIGHT] = direction.x <= 0 ? clamp[CLAMP_RIGHT] : Mathf.Min(clamp[CLAMP_RIGHT], point.x);
        clamp[CLAMP_LEFT] = direction.x >= 0 ? clamp[CLAMP_LEFT] : Mathf.Max(clamp[CLAMP_LEFT], point.x);

        clamp[CLAMP_UP] = direction.y <= 0 ? clamp[CLAMP_UP] : Mathf.Min(clamp[CLAMP_UP], -point.y);
        clamp[CLAMP_DOWN] = direction.y >= 0 ? clamp[CLAMP_DOWN] : Mathf.Max(clamp[CLAMP_DOWN], -point.y);
    }
}
