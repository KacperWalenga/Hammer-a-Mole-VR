using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private List<AudioClip> gameMusics;

    [SerializeField] private DropdownSongSelector DropdownSongSelectorPrefab;
    [SerializeField] private Transform lobbySongsContainer;
    [SerializeField] private Transform gameSongsContainer;
    
    [field:SerializeField] public AudioClip CurrentGameMusic { get; private set; }
    [field:SerializeField] public AudioClip CurrentLobbyMusic { get; private set; }
    
    public event Action CurrentGameMusicChanged;
    public event Action CurrentLobbyMusicChanged;
    public event Action EffectsVolumeChanged;
    public event Action LobbyMusicVolumeChanged;
    public event Action GameMusicVolumeChanged;
    
    private float effectsVolume = 0.5f;
    public float EffectsVolume
    {
        get => effectsVolume;
        set
        {
            if (Mathf.Approximately(effectsVolume, value)) 
                return;
            
            effectsVolume = value;
            EffectsVolumeChanged?.Invoke();
        }
    }

    private float lobbyMusicVolume = 0.5f;
    public float LobbyMusicVolume
    {
        get => lobbyMusicVolume;
        set
        {
            if (Mathf.Approximately(lobbyMusicVolume, value)) 
                return;
            
            lobbyMusicVolume = value;
            LobbyMusicVolumeChanged?.Invoke();
        }
    }

    private float gameMusicVolume = 0.5f;
    public float GameMusicVolume
    {
        get => gameMusicVolume;
        set
        {
            if (Mathf.Approximately(gameMusicVolume, value)) 
                return;
            
            gameMusicVolume = value;
            GameMusicVolumeChanged?.Invoke();
        }
    }

    private void Start()
    {
        foreach (var song in gameMusics)
        {
            var lobbySongs = Instantiate(DropdownSongSelectorPrefab, lobbySongsContainer);
            lobbySongs.Init(song.name, () => OnLobbySongSelected(song), CurrentLobbyMusic == song);
            
            var gameSongs = Instantiate(DropdownSongSelectorPrefab, gameSongsContainer);
            gameSongs.Init(song.name, () => OnGameSongSelected(song), CurrentGameMusic == song);
        }
    }

    private void OnGameSongSelected(AudioClip audioClip)
    {
        CurrentGameMusic = audioClip;
        CurrentGameMusicChanged?.Invoke();
    }

    private void OnLobbySongSelected(AudioClip audioClip)
    {
        CurrentLobbyMusic = audioClip;
        CurrentLobbyMusicChanged?.Invoke();
    }
}