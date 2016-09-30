using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWeave : MonoBehaviour
{
    public TimeLayers timeLayer;

    private float startPerlGravX;
    private float startPerlGravY;
    public float flashSpeed;
    public float factor;

    private Vector2 initGravity;

    void Start()
    {
        startPerlGravX = Random.Range(-10, 10);
        startPerlGravY = Random.Range(-10, 10);
        initGravity = Physics2D.gravity;
    }

    void Update()
    {
        Physics2D.gravity = (Quaternion.Euler(Mathf.PerlinNoise(startPerlGravY, startPerlGravX) * 360f, Mathf.PerlinNoise(startPerlGravX, startPerlGravY)*360f,0) * Physics2D.gravity).normalized * initGravity.magnitude * factor;
        startPerlGravX += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
        startPerlGravY += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
    }
}
