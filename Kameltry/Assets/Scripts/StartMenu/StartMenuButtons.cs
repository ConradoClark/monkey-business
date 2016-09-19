using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButtons : MonoBehaviour
{
    private AsyncOperation op;
    private AsyncOperation unloadOp;
    public void StartGame()
    {
        if (op != null) return;
        op = SceneManager.LoadSceneAsync("Overworld");
    }

    public void Options()
    {

    }

    void Update()
    {
        if (op == null) return;

        if (op.isDone)
        {
            SceneManager.UnloadSceneAsync("Title Screen");
            op = null;
        }
    }
}
