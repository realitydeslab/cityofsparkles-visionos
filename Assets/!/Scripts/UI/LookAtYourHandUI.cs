using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Hands;

[RequireComponent(typeof(LazyFollow))]
public class LookAtYourHandUI : MonoBehaviour
{
    [SerializeField] private float m_FadeDuration = 1f;

    [SerializeField] private float m_Delay = 2f;

    private LazyFollow m_LazyFollow;

    private bool m_Registered = false;

    private bool m_IsLeftHandTracked = false;

    private bool m_IsRightHandTracked = false;

    private CanvasRenderer m_TextCanvasRenderer;

    private void Start()
    {
        m_LazyFollow = GetComponent<LazyFollow>();

        m_TextCanvasRenderer = GetComponentInChildren<CanvasRenderer>();
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
        m_IsLeftHandTracked = true;
        OnHandTrackingUpdated();
    }

    private void OnTrackingAcquired_Right()
    {
        m_IsRightHandTracked = true;
        OnHandTrackingUpdated();
    }

    private void OnTrackingLost_Left()
    {
        m_IsLeftHandTracked = false;
        OnHandTrackingUpdated();
    }

    private void OnTrackingLost_Right()
    {
        m_IsRightHandTracked = false;
        OnHandTrackingUpdated();
    }

    private void OnHandTrackingUpdated()
    {
        if (!m_IsLeftHandTracked && !m_IsRightHandTracked && m_TextCanvasRenderer.GetAlpha() != 1f)
        {
            // Gradually appear
            float duration = m_FadeDuration * (1f - m_TextCanvasRenderer.GetAlpha());
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, m_TextCanvasRenderer.GetAlpha(), 1f, duration)
                .setDelay(m_Delay)
                .setOnUpdate((float alpha) =>
                {
                    m_TextCanvasRenderer.SetAlpha(alpha);
                });
        }
        else if (m_TextCanvasRenderer.GetAlpha() != 0f)
        {
            // Gradually disappear
            float duration = m_FadeDuration * m_TextCanvasRenderer.GetAlpha();
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, m_TextCanvasRenderer.GetAlpha(), 0f, duration)
                //.setDelay(m_Delay)
                .setOnUpdate((float alpha) =>
                {
                    m_TextCanvasRenderer.SetAlpha(alpha);
                });
        }
    }
}
