using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFadeUp : FrostyPoolableObject
{
    public Text[] textRenderers;
    public float fadeSpeed;
    public float upSpeed;
    public TimeLayers timeLayer;

    private float fadeSpeedElapsed;
    private float fadeStep;
    private bool started;

    public float fadeDelay;
    private float fadeDelayElapsed;

    void Start()
    {
        fadeSpeedElapsed = fadeSpeed;
        fadeDelayElapsed = fadeDelay;
        for (int i = 0; i < this.textRenderers.Length; i++)
        {
            Text textRenderer = textRenderers[i];
            textRenderer.color = new Color(textRenderer.color.r, textRenderer.color.g, textRenderer.color.b, 1);
        }
    }

    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y + Toolbox.Instance.time.GetDeltaTime(timeLayer) * upSpeed
            , this.transform.position.z);

        fadeDelayElapsed -= Toolbox.Instance.time.GetDeltaTime(timeLayer);

        if (fadeDelayElapsed > 0) return;

        for (int i = 0; i < this.textRenderers.Length; i++)
        {
            Text textRenderer = textRenderers[i];
            textRenderer.color = new Color(textRenderer.color.r, textRenderer.color.g, textRenderer.color.b, Mathf.Lerp(1, 0, fadeStep));
        }

        fadeStep += fadeSpeed * Toolbox.Instance.time.GetDeltaTime(timeLayer);        

        if (fadeStep > 1)
        {
            Toolbox.Instance.pool.Release(this, this.gameObject);
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        fadeStep = 0f;
        fadeDelayElapsed = fadeDelay;
        for (int i = 0; i < this.textRenderers.Length; i++)
        {
            Text textRenderer = textRenderers[i];
            textRenderer.color = new Color(textRenderer.color.r, textRenderer.color.g, textRenderer.color.b, 1);
        }
    }
}
