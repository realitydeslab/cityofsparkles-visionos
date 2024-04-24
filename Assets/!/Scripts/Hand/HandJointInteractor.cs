using UnityEngine;
using UnityEngine.XR.Hands;

[RequireComponent(typeof(Collider))]
public class HandJointInteractor : MonoBehaviour
{
    [SerializeField] private Handedness m_Handedness;

    [SerializeField] private XRHandJointID m_HandJointID;

    public Handedness Handedness => m_Handedness;

    public XRHandJointID HandJointID => m_HandJointID;

    public IInteractable Interactable
    {
        get => m_Interactable;
        set
        {
            m_Interactable = value;
        }
    }

    private IInteractable m_Interactable;
}
