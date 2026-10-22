using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        Game,
        Lobby,
        Effects
    }

    [SerializeField] private VolumeType volumeType;
    
    private Slider slider;
    private AudioSettings audioSettings;

    private void Start()
    {
        TryGetComponent(out slider);
        audioSettings = FindAnyObjectByType<AudioSettings>();
        
        slider.onValueChanged.AddListener(SliderValueChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(SliderValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        switch (volumeType)
        {
            case VolumeType.Game:
                audioSettings.GameMusicVolume = value;
                break;
            case VolumeType.Lobby:
                audioSettings.LobbyMusicVolume = value;
                break;
            case VolumeType.Effects:
                audioSettings.EffectsVolume = value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
