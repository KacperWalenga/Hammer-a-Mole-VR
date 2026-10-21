using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int pointsPerMole = 10;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;

    public int Score { get; private set; }
    public float Time { get; private set; }

    private bool isGameStarted;

    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StartListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
        EventManager.StartListening(Consts.Events.Mole.Hit, MoleHit);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StopListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);
    }

    private void Update()
    {
        if (!isGameStarted)
            return;

        Time += UnityEngine.Time.deltaTime;
        UpdateTimer();
    }

    private void GamePrepare()
    {
        Score = 0;
        Time = 0;
        
        scoreText.text = Score.ToString("D9");
    }

    private void GameStarted()
    {
        isGameStarted = true;
    }

    private void GameEnded()
    {
        isGameStarted = false;
    }

    private void MoleHit()
    {
        Score += pointsPerMole;
        scoreText.text = Score.ToString("D9");
    }

    private void UpdateTimer()
    {
        timerText.text = $"{Mathf.FloorToInt(Time)}";
    }
}
