using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    public abstract float InteractRange { get; }

    public abstract void Interact();
}
