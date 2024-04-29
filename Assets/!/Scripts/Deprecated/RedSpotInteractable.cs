using UnityEngine;

public class RedSpotInteractable : IInteractable
{
    [SerializeField] private TweetComponent m_TweetPrefab;

    public override float InteractRange => m_InteractRange;

    private float m_InteractRange = 0.15f;

    private HandJointInteractor m_Target;

    private float m_ApproachSpeed = 1.5f;

    private Vector3 m_Offset = new(-10f, 0f, 15f);

    private bool m_Triggered = false;

    private Vector3 m_TriggerPosition;

    private float m_EscapeSpeed = 3f;

    private bool m_IsFadingOut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Target != null)
            return;

        if (other.TryGetComponent<HandJointInteractor>(out var handJoint))
        {
            if (handJoint.HandJointID != UnityEngine.XR.Hands.XRHandJointID.IndexTip)
                return;

            m_Target = handJoint;
        }
    }

    private void Update()
    {
        if (m_IsFadingOut)
            return;

        if (m_Triggered)
        {
            transform.position = Vector3.Lerp(transform.position, m_TriggerPosition, m_EscapeSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_TriggerPosition) < 1f)
            {
                Instantiate(m_TweetPrefab, transform.position, Quaternion.identity);
                m_IsFadingOut = true;
                // TODO: Play fading out animation and destroy the game object
            }
            return;
        }

        if (m_Target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, m_Target.transform.position, m_ApproachSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, m_Target.transform.position);
        if (m_Target.Interactable == null && dist < InteractRange)
            m_Target.Interactable = this;

        if (m_Target.Interactable == this && dist > InteractRange)
            m_Target.Interactable = null;
    }

    public override void Interact()
    {
        if (m_Triggered)
            return;

        Debug.Log($"Interacted with red spot");
        m_Triggered = true;

        Quaternion horizontalRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        m_TriggerPosition = Camera.main.transform.position + horizontalRotation * m_Offset;
    }
}
