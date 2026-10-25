using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private ScoreboardEntry scoreboardEntryPrefab;
    [SerializeField] private Transform scoreboardEntriesContainer;
    
    private ScoreHistoryData scoreHistoryData = new ();
    private ScoreManager scoreManager;
    private DifficultySettings difficultySettings;

    private const int MAX_RESULTS = 10;
    private const string SAVE_KEY = "Scoreboard";

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        difficultySettings = FindAnyObjectByType<DifficultySettings>();

        if (!scoreManager || !difficultySettings)
        {
            Debug.LogWarning("[Scoreboard] ScoreManager or DifficultySettings not found");
            enabled = false;
            return;
        }
        
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
        difficultySettings.DifficultyChanged += RefreshScoreboard;
        
        RefreshScoreboard();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
        
        if (difficultySettings)
            difficultySettings.DifficultyChanged -= RefreshScoreboard;
    }

    private void GameEnded()
    {
        var entry = new ScoreDataEntry
        {
            Score = scoreManager.Score,
            Time = scoreManager.Time,
            DateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
            Difficulty = difficultySettings.CurrentDifficulty
        };
        
        scoreHistoryData.ScoreDataEntries.Add(entry);
        TrimEntries();
        
        Save();
        RefreshScoreboard();
    }

    private void Save()
    {
        var json = JsonUtility.ToJson(scoreHistoryData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        var json = PlayerPrefs.GetString(SAVE_KEY);

        if (!string.IsNullOrEmpty(json))
            scoreHistoryData = JsonUtility.FromJson<ScoreHistoryData>(json);

        TrimEntries();
    }

    private void TrimEntries()
    {
        scoreHistoryData.ScoreDataEntries = scoreHistoryData.ScoreDataEntries
            .GroupBy(x => x.Difficulty)
            .SelectMany(group => group
                .OrderByDescending(x => x.Score)
                .Take(MAX_RESULTS))
            .ToList();
    }

    private void RefreshScoreboard()
    {
        var childCount = scoreboardEntriesContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
            Destroy(scoreboardEntriesContainer.GetChild(i).gameObject);
        
        var entries = scoreHistoryData.ScoreDataEntries
            .Where(x => x.Difficulty == difficultySettings.CurrentDifficulty)
            .OrderByDescending(x => x.Score);
        
        foreach (var scoreDataEntry in entries)
        {
            var scoreboardEntry = Instantiate(scoreboardEntryPrefab, scoreboardEntriesContainer);
            scoreboardEntry.Init(scoreDataEntry);
        }

    }
}
