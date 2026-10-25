using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class HapticsToggle : MonoBehaviour
{
    private Toggle toggle;
    private HapticsManager hapticsManager;

    private void Start()
    {
        TryGetComponent(out toggle);
        hapticsManager = FindAnyObjectByType<HapticsManager>();

        if (!hapticsManager)
        {
            Debug.LogError("[HapticsToggle] HapticsManager not found");
            enabled = false;
            return;
        }

        toggle.isOn = hapticsManager.HapticsEnabled;
        toggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool isOn)
    {
        hapticsManager.HapticsEnabled = isOn;
    }
}
