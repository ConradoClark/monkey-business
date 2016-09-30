using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldButtons : MonoBehaviour {

    private AsyncOperation op;
    public float fadeSpeed = 1;

    public TimeLayers timeLayer;
    private bool loading;

    private float cameraSize;

    void Start()
    {
        cameraSize = Camera.main.orthographicSize;
    }

    public void StartLevel(string levelName)
    {
        if (loading) return;
        loading = true;

        cameraSize = Camera.main.orthographicSize;

        StartCoroutine(StartLevelRoutine(levelName));
    }

    public IEnumerator StartLevelRoutine(string levelName)
    {
        if (op != null) yield break;

        yield return GoWhite();

        op = SceneManager.LoadSceneAsync(levelName);
        loading = false;
    }

    void Update()
    {
        if (op == null) return;

        if (op.isDone)
        {
            SceneManager.UnloadSceneAsync("Overworld");           
            op = null;
            this.enabled = false;
        }
    }

    IEnumerator GoWhite()
    {
        float step = 0;
        while (step < 1)
        {
            Camera.main.orthographicSize = Mathf.Lerp(cameraSize, -1, step);
            step += Toolbox.Instance.time.GetDeltaTime(timeLayer) * fadeSpeed;
            yield return 1;
        }
        yield return 1;
    }
}
