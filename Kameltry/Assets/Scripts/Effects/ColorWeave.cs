using UnityEngine;
using System.Collections;

public class ColorWeave : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public TimeLayers timeLayer;

    private float startPerlR;
    private float startPerlG;
    private float startPerlB;
    public float flashSpeed;

    void Start()
    {
        startPerlR = Random.Range(0, 10);
        startPerlG = Random.Range(0, 10);
        startPerlB = Random.Range(0, 10);
    }

    void Update()
    {
        this.spriteRenderer.color = new Color(0.25f + Mathf.PerlinNoise(startPerlR, startPerlB) * 0.75f,
           0.25f + Mathf.PerlinNoise(startPerlG, startPerlR) * 0.75f,
            0.25f + Mathf.PerlinNoise(startPerlB, startPerlG) * 0.75f, 1);

        startPerlR += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
        startPerlG += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
        startPerlB += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
    }
}
