using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsManager : MonoBehaviour
{
    [SerializeField] private HapticPulse hitPulse = new(0.4f, 0.05f);
    [SerializeField] private HapticPulse goldenHitPulse = new(0.8f, 0.20f);
    [SerializeField] private HapticPulse heartHitPulse = new(0.5f, 0.10f);
    [SerializeField] private HapticPulse lifeLostPulse = new(1f, 0.3f);
    
    private readonly Dictionary<OVRInput.Controller, Coroutine> vibrationCoroutines = new();
    private Hammer hammer;

    private bool hapticsEnabled = true;
    public bool HapticsEnabled
    {
        get => hapticsEnabled;
        set
        {
            if (hapticsEnabled == value)
                return;

            hapticsEnabled = value;

            if (!hapticsEnabled)
                StopAllVibrations();
        }
    }

    private void Start()
    {
        hammer = FindAnyObjectByType<Hammer>();

        if (!hammer)
            Debug.LogWarning("[HapticsManager] Hammer not found, vibrating both controllers");

        EventManager.StartListening(Consts.Events.Mole.Hit, MoleHit);
        EventManager.StartListening(Consts.Events.Mole.Missed, MoleMissed);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);
        EventManager.StopListening(Consts.Events.Mole.Missed, MoleMissed);
    }

    private void OnDisable()
    {
        StopAllVibrations();
    }

    private void MoleHit(object param)
    {
        if (param is not Mole mole)
            return;

        if (mole.Type == MoleType.Bomb)
        {
            Vibrate(OVRInput.Controller.Touch, lifeLostPulse);
            return;
        }

        var pulse = mole.Type switch
        {
            MoleType.Golden => goldenHitPulse,
            MoleType.Heart => heartHitPulse,
            _ => hitPulse
        };

        Vibrate(hammer ? hammer.GetHoldingController() : OVRInput.Controller.Touch, pulse);
    }

    private void MoleMissed()
    {
        Vibrate(OVRInput.Controller.Touch, lifeLostPulse);
    }

    private void Vibrate(OVRInput.Controller controller, HapticPulse pulse)
    {
        if (!hapticsEnabled)
            return;

        if (vibrationCoroutines.TryGetValue(controller, out var coroutine))
            StopCoroutine(coroutine);

        vibrationCoroutines[controller] = StartCoroutine(VibrationCoroutine(controller, pulse));
    }

    private IEnumerator VibrationCoroutine(OVRInput.Controller controller, HapticPulse pulse)
    {
        OVRInput.SetControllerVibration(1, pulse.Amplitude, controller);
        yield return new WaitForSeconds(pulse.Duration);

        OVRInput.SetControllerVibration(0, 0, controller);
        vibrationCoroutines.Remove(controller);
    }

    private void StopAllVibrations()
    {
        foreach (var (controller, coroutine) in vibrationCoroutines)
        {
            StopCoroutine(coroutine);
            OVRInput.SetControllerVibration(0, 0, controller);
        }

        vibrationCoroutines.Clear();
    }
}
