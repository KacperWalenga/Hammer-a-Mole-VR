using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScoreDataEntry
{
    public int Score;
    public float Time;
    public string DateTime;
}

[Serializable]
public class ScoreHistoryData
{
    public List<ScoreDataEntry> ScoreDataEntries = new();
}

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private ScoreboardEntry scoreboardEntryPrefab;
    [SerializeField] private Transform scoreboardEntriesContainer;
    
    private ScoreHistoryData scoreHistoryData = new ();
    private ScoreManager scoreManager;

    private const int MAX_RESULTS = 10;
    private const string SAVE_KEY = "Scoreboard";

    private void Awake()
    {
        Load();
        RefreshScoreboard();
    }

    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
        
        scoreManager = FindAnyObjectByType<ScoreManager>();

        if (scoreManager) 
            return;
        
        Debug.LogWarning("[Scoreboard] ScoreManager not found");
        enabled = false;
    }

    private void GameEnded()
    {
        var entry = new ScoreDataEntry
        {
            Score = scoreManager.Score,
            Time = scoreManager.Time,
            DateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        };
        
        scoreHistoryData.ScoreDataEntries.Add(entry);

        scoreHistoryData.ScoreDataEntries = scoreHistoryData.ScoreDataEntries
            .OrderByDescending(x => x.Score)
            .Take(MAX_RESULTS)
            .ToList();
        
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

        scoreHistoryData.ScoreDataEntries = scoreHistoryData.ScoreDataEntries
            .OrderByDescending(x => x.Score)
            .Take(MAX_RESULTS)
            .ToList();
    }

    private void RefreshScoreboard()
    {
        var childCount = scoreboardEntriesContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
            Destroy(scoreboardEntriesContainer.GetChild(i).gameObject);
        
        foreach (var scoreDataEntry in scoreHistoryData.ScoreDataEntries)
        {
            var scoreboardEntry = Instantiate(scoreboardEntryPrefab, scoreboardEntriesContainer);
            scoreboardEntry.Init(scoreDataEntry);
        }

    }
}
