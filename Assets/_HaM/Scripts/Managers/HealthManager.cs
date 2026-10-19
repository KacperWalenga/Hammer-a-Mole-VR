using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private MeshRenderer[] hearts;

    private int maxHealth => hearts.Length;
    private int currentHealth;
    
    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Prepare, Prepare);
        EventManager.StartListening(Consts.Events.Mole.Missed, RemoveHeart);
    }

    private void RemoveHeart()
    {
        currentHealth -= 1;
        SetHeart(currentHealth, false);
        
        if(currentHealth <= 0)
            EventManager.TriggerEvent(Consts.Events.Game.Ended);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, Prepare);
    }

    private void Prepare()
    {
        currentHealth = maxHealth;

        for (var i = 0; i < maxHealth; i++)
        {
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
