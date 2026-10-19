using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip lobbySound;
    [SerializeField] private AudioClip startGameSound;
    [SerializeField] private AudioClip gameSound;

    [Header("Effects")] 
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip errorSound;
    
    private AudioSource audioSource;
    
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
        
        GameEnded();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, GamePrepare);
        EventManager.StopListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
        
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);
        EventManager.StopListening(Consts.Events.Mole.Missed, MoleMissed);
    }

    private void GameStarted()
    {
        audioSource.clip = gameSound;
        audioSource.Play();
    }

    private void GamePrepare()
    {
        audioSource.clip = startGameSound;
        audioSource.Play();
        
        Invoke(nameof(StartGame), startGameSound.length);
    }

    private void GameEnded()
    {
        audioSource.clip = lobbySound;
        audioSource.Play();
    }

    private void MoleHit()
    {
        audioSource.PlayOneShot(correctSound);
    }

    private void MoleMissed()
    {
        audioSource.PlayOneShot(errorSound);
    }

    private void StartGame()
    {
        EventManager.TriggerEvent(Consts.Events.Game.Started);
    }
}
