using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : FrostyPoolableObject
{
    public LevelStats stats;
    public Text timerSeconds;
    public Text timerSecondsOutline;
    public Text timerDecs;
    public Text timerDecsOutline;

    public Text score;
    public Text scoreOutline;

    public Color greenColor;
    public Color warningColor;
    public Color dangerColor;

    private bool half;
    private bool tenSeconds;
    private float currentTime;

    public float timeScoreFactor;
    private int scorePadLeft;

    public TimeLayers globalTimeLayer;
    public string nextLevel;

    public TurnButton nextLevelButton;
    private bool triggeredNextLevel;

    private bool win;
    public LevelEnd levelEnd;
    void Start()
    {
        Toolbox.Instance.levelManager = this;

        timerSecondsOutline.text = timerSeconds.text = Mathf.FloorToInt(stats.startTimer).ToString() + ".";
        timerSecondsOutline.color = greenColor;

        timerDecsOutline.text = timerDecs.text = Mathf.FloorToInt(((stats.startTimer % 1) * 10)).ToString();
        timerDecsOutline.color = greenColor;

        currentTime = stats.startTimer;
        scorePadLeft = score.text.Length;

        Toolbox.Instance.pool.Cleanup();
        win = false;
    }

    void Update()
    {
        score.text = scoreOutline.text = stats.GetScore().ToString().PadLeft(scorePadLeft, '0');

        currentTime = Mathf.Clamp(stats.GetCurrentTime(),0, currentTime);

        if (!half && currentTime < stats.startTimer / 2f)
        {
            timerSecondsOutline.color = warningColor;
            timerDecsOutline.color = warningColor;
            half = true;
        }

        if (!tenSeconds && currentTime < 10f)
        {
            timerSecondsOutline.color = dangerColor;
            timerDecsOutline.color = dangerColor;
            tenSeconds = true;
        }

        timerSecondsOutline.text = timerSeconds.text = Mathf.FloorToInt(currentTime).ToString() + ".";
        timerDecsOutline.text = timerDecs.text = Mathf.FloorToInt(((currentTime % 1) * 10)).ToString();

        if (!triggeredNextLevel && nextLevelButton.IsPressed)
        {
            triggeredNextLevel = true;
            GoToNextLevel();
        }

        if (IsTimerRunning() && currentTime == 0f)
        {
            StopTimer();
            StartCoroutine(ShowLevelEnd(false));
        }
    }

    public override void ResetState()
    {
        timerSecondsOutline.text = timerSeconds.text = Mathf.FloorToInt(stats.startTimer).ToString() + ".";
        timerSecondsOutline.color = greenColor;

        timerDecsOutline.text = timerDecs.text = Mathf.FloorToInt(((stats.startTimer % 1) * 10)).ToString();
        timerDecsOutline.color = greenColor;
        base.ResetState();
        this.half = this.tenSeconds = false;
    }

    public void StopTimer()
    {
        this.stats.StopTimer();
    }

    public bool IsTimerRunning()
    {
        return this.stats.IsTimerRunning();
    }

    public IEnumerator BlinkTimer()
    {
        for (int i = 0; i < 3; i++)
        {
            timerSecondsOutline.enabled = timerSeconds.enabled = timerDecsOutline.enabled = timerDecs.enabled = false;
            yield return new WaitForSecondsRealtime(0.1f * Toolbox.Instance.time.GetLayerMultiplier(globalTimeLayer));
            timerSecondsOutline.enabled = timerSeconds.enabled = timerDecsOutline.enabled = timerDecs.enabled = true;
            yield return new WaitForSecondsRealtime(0.2f * Toolbox.Instance.time.GetLayerMultiplier(globalTimeLayer));
        }
    }

    public IEnumerator ShowLevelEnd(bool win)
    {
        levelEnd.Show(win);
        yield break;
    }

    public IEnumerator ConvertTimerToScore()
    {
        float time = currentTime;
        float timeScore = time * timeScoreFactor;
        int currentScore = this.stats.GetScore();
        int targetScore = currentScore + (int)timeScore;

        float step = 0;
        while (step < 1)
        {
            this.currentTime = Mathf.Lerp(time, 0, step);
            this.stats.SetScore((int)Mathf.Lerp(currentScore, targetScore, step));
            step += Toolbox.Instance.time.GetDeltaTime(globalTimeLayer)/2f;
            yield return 1;
        }

        this.currentTime = 0;
        this.stats.SetScore(targetScore);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void IncreaseTimer(float time)
    {
        if (!this.IsTimerRunning()) return;        
        this.currentTime += time;
        this.stats.IncreaseTimer(time);
    }
}
