using System;
using UnityEngine;

public class DifficultySettings : MonoBehaviour
{
    [SerializeField] private DifficultyLevel easy = new(2f, 0.9f, 0.01f, 1f, 3, 0.1f, 0f, 0.08f);
    [SerializeField] private DifficultyLevel medium = new(1.5f, 0.7f, 0.015f, 1.5f, 2, 0.08f, 0.1f, 0.05f);
    [SerializeField] private DifficultyLevel hard = new(1f, 0.5f, 0.02f, 2f, 1, 0.06f, 0.2f, 0.03f);

    [Header("UI")]
    [SerializeField] private DropdownSongSelector dropdownElementPrefab;
    [SerializeField] private Transform difficultiesContainer;

    [field:SerializeField] public Difficulty CurrentDifficulty { get; private set; } = Difficulty.Easy;

    public event Action DifficultyChanged;

    public DifficultyLevel CurrentLevel => GetLevel(CurrentDifficulty);

    private bool isGameActive;

    private void Awake()
    {
        foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
        {
            var element = Instantiate(dropdownElementPrefab, difficultiesContainer);
            element.Init(difficulty.ToString(), () => OnDifficultySelected(difficulty), CurrentDifficulty == difficulty);
        }
    }

    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
    }

    private DifficultyLevel GetLevel(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => easy,
            Difficulty.Medium => medium,
            Difficulty.Hard => hard,
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
        };
    }

    private void OnDifficultySelected(Difficulty difficulty)
    {
        if (isGameActive)
        {
            Debug.LogWarning("[DifficultySettings] Difficulty can't be changed during the game");
            return;
        }

        if (CurrentDifficulty == difficulty)
            return;

        CurrentDifficulty = difficulty;
        DifficultyChanged?.Invoke();
    }

    private void GamePrepare()
    {
        isGameActive = true;
    }

    private void GameEnded()
    {
        isGameActive = false;
    }
}
