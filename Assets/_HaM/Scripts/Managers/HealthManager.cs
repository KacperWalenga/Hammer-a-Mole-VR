using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private MeshRenderer[] hearts;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    private DifficultySettings difficultySettings;

    private void Start()
    {
        difficultySettings = FindAnyObjectByType<DifficultySettings>();

        if (!difficultySettings)
        {
            Debug.LogError("[HealthManager] DifficultySettings not found");
            enabled = false;
            return;
        }

        EventManager.StartListening(Consts.Events.Game.Prepare, Prepare);
        EventManager.StartListening(Consts.Events.Mole.Missed, RemoveHeart);
        EventManager.StartListening(Consts.Events.Mole.Hit, MoleHit);

        difficultySettings.DifficultyChanged += UpdateHearts;
        UpdateHearts();
    }

    private void MoleHit(object param)
    {
        if (param is not Mole mole)
            return;

        switch (mole.Type)
        {
            case MoleType.Bomb:
                RemoveHeart();
                break;
            case MoleType.Heart:
                AddHeart();
                break;
        }
    }

    private void RemoveHeart()
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= 1;
        SetHeart(CurrentHealth, false);

        if(CurrentHealth <= 0)
            EventManager.TriggerEvent(Consts.Events.Game.Ended);
    }

    private void AddHeart()
    {
        if (CurrentHealth >= MaxHealth)
            return;

        SetHeart(CurrentHealth, true);
        CurrentHealth += 1;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, Prepare);
        EventManager.StopListening(Consts.Events.Mole.Missed, RemoveHeart);
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);

        if (difficultySettings)
            difficultySettings.DifficultyChanged -= UpdateHearts;
    }

    private void Prepare()
    {
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        MaxHealth = Mathf.Min(difficultySettings.CurrentLevel.MaxHealth, hearts.Length);
        CurrentHealth = MaxHealth;

        for (var i = 0; i < hearts.Length; i++)
        {
            var isAvailable = i < MaxHealth;

            hearts[i].gameObject.SetActive(isAvailable);

            if (isAvailable)
                SetHeart(i, true);
        }
    }

    private void SetHeart(int index, bool state)
    {
        var heart = hearts[index];
        var materials = heart.sharedMaterials;

        for (var i = 0; i < materials.Length; i++)
            materials[i] = state ? enabledMaterial : disabledMaterial;

        heart.sharedMaterials = materials;
    }
}
