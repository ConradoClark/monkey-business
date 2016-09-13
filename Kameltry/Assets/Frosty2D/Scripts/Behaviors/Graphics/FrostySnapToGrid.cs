using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Frosty-Rendering/SnapToGrid")]
public class FrostySnapToGrid : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Editor Snapping")]
    public bool snapToGrid = true;
    public Vector2 snapValue = Vector2.one;
    public Vector2 gridOffset = Vector2.zero;

    private void Update()
    {

        if (!Application.isPlaying)
        {
            // Adjust size and position
            if (snapToGrid)
                transform.position = RoundTransform(transform.position, snapValue);
        }
    }

    // The snapping code
    private Vector3 RoundTransform(Vector3 v, Vector2 snapValue)
    {
        return new Vector3
        (
            snapValue.x > 0 ? snapValue.x * Mathf.Round((v.x - gridOffset.x) / snapValue.x) + gridOffset.x : v.x,
            snapValue.y > 0 ? snapValue.y * Mathf.Round((v.y - gridOffset.y) / snapValue.y) + gridOffset.y : v.y,
            v.z
        );
    }
#endif
}
