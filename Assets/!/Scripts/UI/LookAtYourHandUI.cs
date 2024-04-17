using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Hands;

[RequireComponent(typeof(LazyFollow))]
public class LookAtYourHandUI : MonoBehaviour
{
    private LazyFollow m_LazyFollow;

    private bool m_Registered = false;

    private void Start()
    {
        m_LazyFollow = GetComponent<LazyFollow>();
    }

    private void Update()
    {
        if (m_LazyFollow.target == null)
        {
            m_LazyFollow.target = Camera.main.transform;
        }

        if (!m_Registered)
        {
            var hands = FindObjectsOfType<XRHandTrackingEvents>();
            int registeredHandCount = 0;
            foreach (var hand in hands)
            {
                if (hand.handedness == Handedness.Left)
                {
                    hand.trackingAcquired.AddListener(OnTrackingAcquired_Left);
                    hand.trackingLost.AddListener(OnTrackingLost_Left);
                    registeredHandCount++;
                }
                else if (hand.handedness == Handedness.Right)
                {
                    hand.trackingAcquired.AddListener(OnTrackingAcquired_Right);
                    hand.trackingLost.AddListener(OnTrackingLost_Right);
                    registeredHandCount++;
                }
            }

            m_Registered = registeredHandCount == 2;
        }
    }

    private void OnDestroy()
    {
        if (m_Registered)
        {
            var hands = FindObjectsOfType<XRHandTrackingEvents>();
            foreach (var hand in hands)
            {
                if (hand.handedness == Handedness.Left)
                {
                    hand.trackingAcquired.RemoveListener(OnTrackingAcquired_Left);
                    hand.trackingLost.RemoveListener(OnTrackingLost_Left);
                }
                else if (hand.handedness == Handedness.Right)
                {
                    hand.trackingAcquired.RemoveListener(OnTrackingAcquired_Right);
                    hand.trackingLost.RemoveListener(OnTrackingLost_Right);
                }
            }
        }
    }

    private void OnTrackingAcquired_Left()
    {
        Debug.Log("cc: OnTrackingAcquired_Left");
    }

    private void OnTrackingAcquired_Right()
    {
        Debug.Log("cc: OnTrackingAcquired_Right");
    }

    private void OnTrackingLost_Left()
    {
        Debug.Log("cc: OnTrackingLost_Left");
    }

    private void OnTrackingLost_Right()
    {
        Debug.Log("cc: OnTrackingLost_Right");
    }
}
