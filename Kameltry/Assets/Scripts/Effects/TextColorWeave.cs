using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorWeave : MonoBehaviour {

    public Text textRenderer;
    public TimeLayers timeLayer;

    private float startPerlR;
    private float startPerlG;
    private float startPerlB;
    public float flashSpeed;

    public float min;
    public float max;

    void Start()
    {
        startPerlR = Random.Range(0, 10);
        startPerlG = Random.Range(0, 10);
        startPerlB = Random.Range(0, 10);
    }

    void Update()
    {
        this.textRenderer.color = new Color(0.25f + Mathf.PerlinNoise(startPerlR, startPerlB) * (max-min),
           min + Mathf.PerlinNoise(startPerlG, startPerlR) * (max - min),
            min + Mathf.PerlinNoise(startPerlB, startPerlG) * (max - min), this.textRenderer.color.a);

        startPerlR += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
        startPerlG += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
        startPerlB += Toolbox.Instance.time.GetDeltaTime(timeLayer) * flashSpeed;
    }
}
