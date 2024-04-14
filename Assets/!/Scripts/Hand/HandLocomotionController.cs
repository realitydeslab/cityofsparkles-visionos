using UnityEngine;
using UnityEngine.XR.Hands;

public class HandLocomotionController : MonoBehaviour
{
    [SerializeField] private HandGestureManager m_HandGestureManager;

    [SerializeField] private HandTrackingManager m_HandTrackingManager;

    [SerializeField] private float m_Acceleration = 2f;

    [SerializeField] private float m_MaxSpeed = 10f;

    private bool m_IsFlying = false;

    private Handedness m_DirectionHandedness;

    private Vector3 m_Direction;

    private float m_Speed;

    public void OnHandGestureChanged(Handedness handedness, HandGesture oldGesture, HandGesture newGesture)
    {
        Handedness oppositeHandedness = handedness == Handedness.Left ? Handedness.Right : Handedness.Left;
        if (newGesture == HandGesture.Pinching && m_HandGestureManager.HandGestures[oppositeHandedness] != HandGesture.Pinching)
        {
            // Start flying
            m_DirectionHandedness = oppositeHandedness;
            m_IsFlying = true;
            return;
        }

        if (oldGesture == HandGesture.Pinching && m_HandGestureManager.HandGestures[oppositeHandedness] != HandGesture.Pinching)
        {
            // Stop flying
            m_DirectionHandedness = Handedness.Invalid;
            m_IsFlying = false;
            return;
        }
    }

    private void Update()
    {
        if (!m_IsFlying)
            return;

        UpdateDirection();

        // Update speed
        if (m_Speed < m_MaxSpeed)
        {
            m_Speed += m_Acceleration * Time.deltaTime;
            m_Speed = Mathf.Min(m_Speed, m_MaxSpeed);
        }

        // Update position
        transform.position += m_Speed * Time.time * m_Direction;
    }

    private void UpdateDirection()
    {
        var thumbMetacarpal = m_HandTrackingManager.Hands[m_DirectionHandedness].GetHandJointPose(XRHandJointID.ThumbMetacarpal);
        var indexMetacarpal = m_HandTrackingManager.Hands[m_DirectionHandedness].GetHandJointPose(XRHandJointID.IndexMetacarpal);

        m_Direction = (indexMetacarpal.position - thumbMetacarpal.position).normalized;
    }
}
