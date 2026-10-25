using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private HandGrabInteractable[] handGrabInteractables;
    [SerializeField] private GrabInteractable[] grabInteractables;


    private void Awake()
    {
        var grabbable = GetComponentInParent<Grabbable>();
        var searchRoot = grabbable ? grabbable.transform : transform;

        if (handGrabInteractables == null || handGrabInteractables.Length == 0)
            handGrabInteractables = searchRoot.GetComponentsInChildren<HandGrabInteractable>(true);

        if (grabInteractables == null || grabInteractables.Length == 0)
            grabInteractables = searchRoot.GetComponentsInChildren<GrabInteractable>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        var mole = other.GetComponentInParent<Mole>();

        if (!mole)
            return;

        mole.Hit();
    }
    
    public OVRInput.Controller GetHoldingController()
    {
        foreach (var interactable in handGrabInteractables)
        {
            foreach (var interactor in interactable.SelectingInteractors)
            {
                if (interactor.Hand != null)
                    return ToController(interactor.Hand.Handedness);
            }
        }

        foreach (var interactable in grabInteractables)
        {
            foreach (var interactor in interactable.SelectingInteractors)
            {
                var controller = interactor.GetComponentInParent<IController>();

                if (controller != null)
                    return ToController(controller.Handedness);

                var hand = interactor.GetComponentInParent<IHand>();

                if (hand != null)
                    return ToController(hand.Handedness);
            }
        }

        return OVRInput.Controller.Touch;
    }

    private static OVRInput.Controller ToController(Handedness handedness)
    {
        return handedness == Handedness.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
    }
}
