using System.Collections;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Color enabledButtonColor = new (255, 0, 0);
    [SerializeField] private Color disabledButtonColor = new (50, 0, 0);
    [SerializeField] private InteractableUnityEventWrapper interactableUnityEventWrapper;
    [SerializeField] private float blinkingSpeed = 0.5f;

    private bool isActive = true;
    private Coroutine blinkingCoroutine;
    
    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
        interactableUnityEventWrapper.WhenSelect.AddListener(ButtonPressed);

        blinkingCoroutine = StartCoroutine(StartBlinking());
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
        interactableUnityEventWrapper.WhenSelect.RemoveListener(ButtonPressed);
    }

    private void GameEnded()
    {
        isActive = true;
        blinkingCoroutine = StartCoroutine(StartBlinking());
    }

    private void ButtonPressed()
    {
        EventManager.TriggerEvent(Consts.Events.Game.Prepare);
        
        StopCoroutine(blinkingCoroutine);
        blinkingCoroutine = null;
        
        buttonText.color = disabledButtonColor;
        isActive = false;
    }

    private IEnumerator StartBlinking()
    {
        while (isActive)
        {
            buttonText.color = enabledButtonColor;
            yield return new WaitForSeconds(blinkingSpeed);
            
            buttonText.color = disabledButtonColor;
            yield return new WaitForSeconds(blinkingSpeed);
        }
    }
}
