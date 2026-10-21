using DG.Tweening;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private Vector3 moveOffset = new (0, 0.1f, 0);
    [SerializeField] private float moveDuration = 0.1f;

    private Vector3 defaultPosition;
    private bool isRaised;
    private Tween moveTween;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
    }

    private void Start()
    {
        EventManager.StartListening(Consts.Events.Mole.Missed, Missed);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Mole.Missed, Missed);
    }

    private void Missed(object mole)
    {
        if (!ReferenceEquals(mole, this))
            return;
        
        Lower();
    }

    public void Hit()
    {
        if (!isRaised)
            return;
        
        Lower();
        
        EventManager.TriggerEvent(Consts.Events.Mole.Hit);
        EventManager.TriggerEvent(Consts.Events.Mole.Hit, this);
    }

    public void Raise()
    {
        isRaised = true;
        MoveTo(defaultPosition + moveOffset);
    }

    public void Lower()
    {
        isRaised = false;
        MoveTo(defaultPosition);
    }

    private void MoveTo(Vector3 target)
    {
        moveTween?.Kill();

        moveTween = transform.DOLocalMove(target, moveDuration)
            .SetEase(Ease.OutExpo);
    }
}
