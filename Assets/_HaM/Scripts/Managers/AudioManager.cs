using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Effects")] 
    [SerializeField] private AudioClip startGameSound;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip errorSound;
    
    private AudioSource audioSource;
    private AudioSettings audioSettings;

    private bool isGameStarted;
    
    private void Awake()
    {
        TryGetComponent(out audioSource);
    }

    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StartListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
        
        EventManager.StartListening(Consts.Events.Mole.Hit, MoleHit);
        EventManager.StartListening(Consts.Events.Mole.Missed, MoleMissed);

        audioSettings = FindAnyObjectByType<AudioSettings>();
        
        if(!audioSettings)
        {
            Debug.LogError("[AudioManager] AudioSettings not found");
            enabled = false;
        }
        
        audioSettings.CurrentGameMusicChanged += AudioSettingsOnCurrentGameMusicChanged;
        audioSettings.CurrentLobbyMusicChanged += AudioSettingsOnCurrentLobbyMusicChanged;
        audioSettings.GameMusicVolumeChanged += AudioSettingsOnGameMusicVolumeChanged;
        audioSettings.LobbyMusicVolumeChanged += AudioSettingsOnLobbyMusicVolumeChanged;
        
        GameEnded();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StopListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
        
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);
        EventManager.StopListening(Consts.Events.Mole.Missed, MoleMissed);
        
        audioSettings.CurrentGameMusicChanged -= AudioSettingsOnCurrentGameMusicChanged;
        audioSettings.GameMusicVolumeChanged -= AudioSettingsOnGameMusicVolumeChanged;
        audioSettings.LobbyMusicVolumeChanged -= AudioSettingsOnLobbyMusicVolumeChanged;
    }

    private void AudioSettingsOnLobbyMusicVolumeChanged()
    {
        if (isGameStarted)
            return;
        
        audioSource.volume = audioSettings.LobbyMusicVolume;
    }

    private void AudioSettingsOnGameMusicVolumeChanged()
    {
        if (!isGameStarted)
            return;
        
        audioSource.volume = audioSettings.GameMusicVolume;
    }

    private void AudioSettingsOnCurrentGameMusicChanged()
    {
        if (!isGameStarted)
            return;
        
        audioSource.clip = audioSettings.CurrentGameMusic;
        audioSource.Play();
    }

    private void AudioSettingsOnCurrentLobbyMusicChanged()
    {
        if (isGameStarted)
            return;
        
        audioSource.clip = audioSettings.CurrentLobbyMusic;
        audioSource.Play();
    }

    private void GameStarted()
    {
        isGameStarted = true;

        audioSource.volume = audioSettings.GameMusicVolume;
        audioSource.clip = audioSettings.CurrentGameMusic;
        audioSource.Play();
    }

    private void GamePrepare()
    {
        isGameStarted = false;
        
        audioSource.volume = audioSettings.EffectsVolume;
        audioSource.clip = startGameSound;
        audioSource.Play();
        
        Invoke(nameof(StartGame), startGameSound.length);
    }

    private void GameEnded()
    {
        isGameStarted = false;
        
        audioSource.volume = audioSettings.LobbyMusicVolume;
        audioSource.clip = audioSettings.CurrentLobbyMusic;
        audioSource.Play();
    }

    private void MoleHit()
    {
        audioSource.PlayOneShot(correctSound, audioSettings.EffectsVolume);
    }

    private void MoleMissed()
    {
        audioSource.PlayOneShot(errorSound, audioSettings.EffectsVolume);
    }

    private void StartGame()
    {
        EventManager.TriggerEvent(Consts.Events.Game.Started);
    }
}
