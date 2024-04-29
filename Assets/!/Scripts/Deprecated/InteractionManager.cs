using UnityEngine;
using UnityEngine.XR.Hands;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private HandJointInteractor[] m_Interactors;

    public bool HasInteractable()
    {
        foreach (var interactor in m_Interactors)
        {
            if (interactor.Interactable != null)
                return true;
        }

        return false;
    }

    public void OnHandGestureChanged(Handedness handedness, HandGesture oldGesture, HandGesture newGesture)
    {
        if (newGesture == HandGesture.Pinching)
        {
            foreach (var interactor in m_Interactors)
            {
                if (interactor.Interactable != null && interactor.Handedness == handedness)
                    interactor.Interactable.Interact();
            }
        }
    }
}
