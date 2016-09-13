using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FrostyTiledSprite))]
[CanEditMultipleObjects]
public class TiledSpriteEditor : Editor{
    void OnEnable()
    {
        var castedTarget = (target as FrostyTiledSprite);
        var meshRenderer = castedTarget.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.hideFlags = HideFlags.HideInInspector;
        }

        var meshFilter = castedTarget.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.hideFlags = HideFlags.HideInInspector;
        }
    }
}
