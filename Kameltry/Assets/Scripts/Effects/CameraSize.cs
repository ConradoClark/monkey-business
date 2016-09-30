using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    public new Camera camera;
    private float step;
    private float currentCameraSize;
    public float initialCameraSize;

    public TimeLayers timeLayer;
    void Start()
    {
        currentCameraSize = camera.orthographicSize;
        camera.orthographicSize = initialCameraSize;
        if (Toolbox.Instance.galaxyCamera == null)
        {
            camera.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    void Update()
    {
        if (step < 1)
        {
            camera.orthographicSize = Mathf.SmoothStep(initialCameraSize, currentCameraSize, step);
            step += Toolbox.Instance.time.GetDeltaTime(timeLayer);
        }
    }
}
