using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class HandTrackingManager : MonoBehaviour
{
    [SerializeField] private Hand m_LeftHand;

    [SerializeField] private Hand m_RightHand;

    [SerializeField] private bool m_HandJointsVisibility = true;

    // True if the left hand is currently detected
    public bool HasLeftHand => m_LeftHand.gameObject.activeSelf;

    // True if the right hand is currently detected
    public bool HasRightHand => m_RightHand.gameObject.activeSelf;

    public Dictionary<Handedness, Hand> Hands => m_Hands;

    private readonly Dictionary<Handedness, Hand> m_Hands = new();

    private XRHandSubsystem m_HandSubsystem;

    // Invoked when hand is detected
    public UnityEvent<Handedness> OnTrackingAcquired;

    // Invoked when tracking is lost
    public UnityEvent<Handedness> OnTrackingLost;

    public UnityEvent<Hand> OnUpdatedHand;

    private void Start()
    {
        m_LeftHand.gameObject.SetActive(false);
        m_RightHand.gameObject.SetActive(false);

        m_Hands.Add(Handedness.Left, m_LeftHand);
        m_Hands.Add(Handedness.Right, m_RightHand);

        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
            meshRenderer.enabled = m_HandJointsVisibility;
    }

    private void Update()
    {
        if (m_HandSubsystem != null && !m_HandSubsystem.running)
        {
            UnsubscribeHandSubsystem();
            return;
        }

        if (m_HandSubsystem == null)
        {
            List<XRHandSubsystem> subsystems = new();
            SubsystemManager.GetSubsystems(subsystems);
            foreach (var subsystem in subsystems)
            {
                if (subsystem.running)
                {
                    m_HandSubsystem = subsystem;
                    break;
                }
            }

            if (m_HandSubsystem != null)
            {
                SubscribeHandSubsystem();
            }
        }
    }

    private void SubscribeHandSubsystem()
    {
        if (m_HandSubsystem == null)
            return;

        m_HandSubsystem.trackingAcquired += OnTrackingAcquiredInternal;
        m_HandSubsystem.trackingLost += OnTrackingLostInternal;
        m_HandSubsystem.updatedHands += OnUpdatedHandsInternal;
    }

    private void UnsubscribeHandSubsystem()
    {
        if (m_HandSubsystem == null)
            return;

        m_HandSubsystem.trackingAcquired -= OnTrackingAcquiredInternal;
        m_HandSubsystem.trackingLost -= OnTrackingLostInternal;
        m_HandSubsystem.updatedHands -= OnUpdatedHandsInternal;
    }

    private void OnTrackingAcquiredInternal(XRHand hand)
    {
        m_Hands[hand.handedness].gameObject.SetActive(true);

        OnTrackingAcquired?.Invoke(hand.handedness);
    }

    private void OnTrackingLostInternal(XRHand hand)
    {
        m_Hands[hand.handedness].gameObject.SetActive(false);

        OnTrackingLost?.Invoke(hand.handedness);
    }

    private void OnUpdatedHandsInternal(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        Handedness handedness = Handedness.Invalid;
        if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints) != 0)
        {
            handedness = Handedness.Left;
            for (int i = (int)XRHandJointID.Wrist; i < (int)XRHandJointID.EndMarker; i++)
            {
                XRHandJointID handJointID = (XRHandJointID)i;
                XRHand hand = handedness == Handedness.Left ? subsystem.leftHand : subsystem.rightHand;
                XRHandJoint handJoint = hand.GetJoint(handJointID);
                if (handJoint.TryGetPose(out var handJointPose))
                {
                    m_Hands[handedness].SetHandJointPose(handJointID, handJointPose);
                }
            }
            OnUpdatedHand?.Invoke(m_Hands[handedness]);
        }

        if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.RightHandJoints) != 0)
        {
            handedness = Handedness.Right;
            for (int i = (int)XRHandJointID.Wrist; i < (int)XRHandJointID.EndMarker; i++)
            {
                XRHandJointID handJointID = (XRHandJointID)i;
                XRHand hand = handedness == Handedness.Left ? subsystem.leftHand : subsystem.rightHand;
                XRHandJoint handJoint = hand.GetJoint(handJointID);
                if (handJoint.TryGetPose(out var handJointPose))
                {
                    m_Hands[handedness].SetHandJointPose(handJointID, handJointPose);
                }
            }
            OnUpdatedHand?.Invoke(m_Hands[handedness]);
        }
    }
}
