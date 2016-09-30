using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButtons : MonoBehaviour
{
    private AsyncOperation op;
    private AsyncOperation unloadOp;

    public SpriteRenderer startGameButton;
    public SpriteRenderer startGameButton_Inner;
    public SpriteRenderer optionsButton;
    public SpriteRenderer optionsButton_Inner;
    public float fadeSpeed = 1;

    public TimeLayers timeLayer;
    private bool loading;

    private float cameraSize;

    void Start()
    {
        cameraSize = Camera.main.orthographicSize;
    }

    public void StartGame()
    {
        if (loading) return;
        loading = true;

        StartCoroutine(StartGameRoutine());
    }

    public IEnumerator StartGameRoutine()
    {
        if (op != null) yield break;

        yield return GoWhite(startGameButton,startGameButton_Inner);

        op = SceneManager.LoadSceneAsync("Overworld");
        loading = false;
    }

    public void Options()
    {

    }

    void Update()
    {
        if (op == null) return;

        if (op.isDone)
        {
            unloadOp = SceneManager.UnloadSceneAsync("Title Screen");
            op = null;
        }
    }

    IEnumerator GoWhite(SpriteRenderer button, SpriteRenderer inner)
    {
        float step = 0;
        Color initColor = button.color;
        while (step < 1)
        {
            Camera.main.orthographicSize = Mathf.Lerp(cameraSize, -1, step);
            button.color = Color.Lerp(initColor, new Color(0, 0, 0, 0), step);
            inner.material.SetFloat("_LevelsMaxInput", Mathf.Lerp(255, 0, step));
            step += Toolbox.Instance.time.GetDeltaTime(timeLayer) * fadeSpeed;
            yield return 1;
        }
        yield return 1;
    }
}
