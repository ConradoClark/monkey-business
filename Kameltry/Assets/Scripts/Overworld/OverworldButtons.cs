using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldButtons : MonoBehaviour {

    private AsyncOperation op;

    public void StartLevel(string levelName)
    {
        if (op != null) return;
        op = SceneManager.LoadSceneAsync(levelName);
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
}
