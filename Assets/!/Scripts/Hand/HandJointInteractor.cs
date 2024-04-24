using UnityEngine;
using UnityEngine.XR.Hands;
using System;

[RequireComponent(typeof(Collider))]
public class HandJointInteractor : MonoBehaviour
{
    [SerializeField] private Handedness m_Handedness;

    [SerializeField] private XRHandJointID m_HandJointID;

    public Handedness Handedness => m_Handedness;

    public XRHandJointID HandJointID => m_HandJointID;

    public IInteractable Interactable => m_Interactable;

    private IInteractable m_Interactable;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Interactable != null)
            return;

        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            m_Interactable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Interactable == null)
            return;

        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            m_Interactable = interactable;
        }
    }
}
