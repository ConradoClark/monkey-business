using UnityEngine;
using System.Collections;

public class LevelStats : MonoBehaviour
{
    public float startTimer;
    private float currentTime;
    public TimeLayers timeLayer;

    private int totalPellets;
    private int collectedPellets;

    private int score;
    private bool timerRunning;

    void Start()
    {
        currentTime = startTimer;
        collectedPellets = 0;
        timerRunning = true;
    }

    void Update()
    {
        if (!timerRunning) return;
        currentTime -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void AddPellet()
    {
        this.totalPellets++;
    }

    public void CollectPellect(int pelletValue)
    {
        this.collectedPellets++;
        score += pelletValue;
    }

    public int GetCollectedPellets()
    {
        return collectedPellets;
    }

    public int GetTotalPellets()
    {
        return totalPellets;
    }

    public int GetScore()
    {
        return score;
    }
    
    public void SetScore(int score)
    {
        this.score = score;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public bool IsTimerRunning()
    {
        return timerRunning;
    }

    public void IncreaseTimer(float time)
    {
        this.currentTime += time;
    }
}
