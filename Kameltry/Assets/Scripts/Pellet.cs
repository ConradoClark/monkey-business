using UnityEngine;
using System.Collections;

public class Pellet : MonoBehaviour
{
    private LevelManager levelManager;
    public SpriteRenderer spriteRenderer;
    public TimeLayers timeLayer;
    public FrostyPoolableObject bonusTextInstance;

    private float startPerlR;
    private float startPerlG;
    private float startPerlB;
    public float flashSpeed;
    public int pelletValue;

    void Start()
    {
        levelManager = Toolbox.Instance.levelManager;
        levelManager.stats.AddPellet();
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            levelManager.stats.CollectPellect(pelletValue);
            GameObject text = Toolbox.Instance.pool.Retrieve(bonusTextInstance);
            text.transform.SetParent(Toolbox.Instance.overUI.canvas.transform, false);
            text.transform.position = Toolbox.Instance.overUI.overUICamera.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(this.transform.position));
            Toolbox.Instance.levelManager.IncreaseTimer(1f);

            GameObject.Destroy(this.gameObject);
        }
    }
}
