using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityWeave : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public TimeLayers timeLayer;

    private float startPerlA;
    public float flashSpeed;

    void Start()
    {
        startPerlA = Random.Range(0, 10);
    }

    void Update()
    {
        this.spriteRenderer.color = new Color(this.spriteRenderer.color.r, this.spriteRenderer.color.g, this.spriteRenderer.color.b, 0.25f + Mathf.PerlinNoise(startPerlA, startPerlA) * 0.75f);

        startPerlA += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
    }
}
