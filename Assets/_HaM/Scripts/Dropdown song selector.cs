using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DropdownSongSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private Toggle toggle;
    private Action onSelected;

    private void Awake()
    {
        TryGetComponent(out toggle);
        
        var toggleGroup = GetComponentInParent<ToggleGroup>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool isOn)
    {
        if (!isOn)
            return;
        
        onSelected?.Invoke();
    }

    public void Init(string text, Action onSelect, bool isOn)
    {
        label.text = text;
        onSelected += onSelect;
        toggle.isOn = isOn;
    }
}
