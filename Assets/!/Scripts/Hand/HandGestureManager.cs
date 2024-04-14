using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;
using UnityEngine.Events;

public enum HandGesture
{
    NotTracked = 0,
    None = 1,
    Pinching = 2
}

public class HandGestureManager : MonoBehaviour
{
    public Dictionary<Handedness, HandGesture> HandGestures => m_HandGestures;

    private readonly Dictionary<Handedness, HandGesture> m_HandGestures = new();

    private const float PINCH_DIST = 0.05f;

    public UnityEvent<Handedness, HandGesture, HandGesture> OnHandGestureChanged;

    private void Start()
    {
        m_HandGestures.Add(Handedness.Left, HandGesture.NotTracked);
        m_HandGestures.Add(Handedness.Right, HandGesture.NotTracked);
    }

    public void OnTrackingAcquired(Handedness handedness)
    {
        OnHandGestureChangedInternal(handedness, HandGesture.None);
    }

    public void OnTrackingLost(Handedness handedness)
    {
        OnHandGestureChangedInternal(handedness, HandGesture.NotTracked);
    }

    public void OnUpdatedHand(Hand hand)
    {
        if (ValidateHandGesture_Pinching(hand))
        {
            OnHandGestureChangedInternal(hand.Handedness, HandGesture.Pinching);
            return;
        }

        OnHandGestureChangedInternal(hand.Handedness, HandGesture.None);
    }

    private void OnHandGestureChangedInternal(Handedness handedness, HandGesture handGesture)
    {
        if (m_HandGestures[handedness] == handGesture)
            return;

        HandGesture prevGesture = m_HandGestures[handedness];
        m_HandGestures[handedness] = handGesture;
        OnHandGestureChanged?.Invoke(handedness, prevGesture, handGesture);
    }

    private bool ValidateHandGesture_Pinching(Hand hand)
    {
        var thumbTip = hand.GetHandJointPose(XRHandJointID.ThumbTip);
        var indexTip = hand.GetHandJointPose(XRHandJointID.IndexTip);

        return Vector3.Distance(thumbTip.position, indexTip.position) < PINCH_DIST;
    }
}
