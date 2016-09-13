using UnityEngine;
using System.Collections;

public class LevelGoal : MonoBehaviour
{
    private bool crossed;
    public TimeLayers globalTimeLayer;
    public TimeLayers gameTimeLayer;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!crossed && other.CompareTag("Player"))
        {
            crossed = true;
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        Toolbox.Instance.levelManager.StopTimer();

        yield return Toolbox.Instance.levelManager.BlinkTimer();
        yield return Toolbox.Instance.levelManager.ConvertTimerToScore();
        yield return Toolbox.Instance.levelManager.ShowLevelEnd();

        yield break;
    }
}
