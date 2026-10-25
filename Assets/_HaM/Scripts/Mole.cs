using DG.Tweening;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private Vector3 moveOffset = new (0, 0.1f, 0);
    [SerializeField] private float moveDuration = 0.1f;

    [Header("Types")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material goldenMaterial;
    [SerializeField] private Material bombMaterial;
    [SerializeField] private Material heartMaterial;

    public MoleType Type { get; private set; }

    private Vector3 defaultPosition;
    private bool isRaised;
    private Tween moveTween;

    private const int HAT_MATERIAL_INDEX = 4;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
    }

    public void Hit()
    {
        if (!isRaised)
            return;
        
        EventManager.TriggerEvent(Consts.Events.Mole.Hit);
        EventManager.TriggerEvent(Consts.Events.Mole.Hit, this);

        Lower();
    }

    public void Raise(MoleType type = MoleType.Normal)
    {
        Type = type;
        SetMaterial(GetMaterial(type));

        isRaised = true;
        MoveTo(defaultPosition + moveOffset);
    }

    public void Lower()
    {
        isRaised = false;
        MoveTo(defaultPosition);
        
        if(Type == MoleType.Normal)
            return;

        Type = MoleType.Normal;
        SetMaterial(GetMaterial(Type));
    }

    private void MoveTo(Vector3 target)
    {
        moveTween?.Kill();

        moveTween = transform.DOLocalMove(target, moveDuration)
            .SetEase(Ease.OutExpo);
    }

    private Material GetMaterial(MoleType type)
    {
        var material = type switch
        {
            MoleType.Golden => goldenMaterial,
            MoleType.Bomb => bombMaterial,
            MoleType.Heart => heartMaterial,
            _ => normalMaterial
        };

        return material ? material : normalMaterial;
    }

    private void SetMaterial(Material material)
    {
        if (!meshRenderer || !material)
            return;

        var materials = meshRenderer.sharedMaterials;
        materials[HAT_MATERIAL_INDEX] = material;

        meshRenderer.sharedMaterials = materials;
    }
}
