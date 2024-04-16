using UnityEngine;
using UnityEngine.XR.Hands;

public class HandLocomotionController : MonoBehaviour
{
    [SerializeField] private HandGestureManager m_HandGestureManager;

    [SerializeField] private Transform m_XROrigin;

    [SerializeField] private Transform m_LeftThumbMetacarpal;

    [SerializeField] private Transform m_LeftIndexProximal;

    [SerializeField] private Transform m_RightThumbMetacarpal;

    [SerializeField] private Transform m_RightIndexProximal;

    [SerializeField] private float m_Acceleration = 1f;

    [SerializeField] private float m_MaxSpeed = 1f;

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
            Debug.Log("Start flying");
            return;
        }

        if (oldGesture == HandGesture.Pinching && m_HandGestureManager.HandGestures[oppositeHandedness] != HandGesture.Pinching)
        {
            // Stop flying
            m_DirectionHandedness = Handedness.Invalid;
            m_IsFlying = false;
            m_Speed = 0f;
            Debug.Log("Stop flying");
            return;
        }
    }

    private void Update()
    {
        if (!m_IsFlying)
            return;

        if (m_HandGestureManager.HandGestures[m_DirectionHandedness] == HandGesture.NotTracked)
            return;

        UpdateDirection();

        // Update speed
        if (m_Speed < m_MaxSpeed)
        {
            m_Speed += m_Acceleration * Time.deltaTime;
            m_Speed = Mathf.Min(m_Speed, m_MaxSpeed);
        }

        // Update position
        m_XROrigin.position += m_Direction * m_Speed * Time.deltaTime;
    }

    private void UpdateDirection()
    {
        if (m_DirectionHandedness == Handedness.Left)
        {
            m_Direction = (m_LeftIndexProximal.position - m_LeftThumbMetacarpal.position).normalized;
        }
        else if (m_DirectionHandedness == Handedness.Right)
        {
            m_Direction = (m_RightIndexProximal.position - m_RightThumbMetacarpal.position).normalized;
        }
    }
}
