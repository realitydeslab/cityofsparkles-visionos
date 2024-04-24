using UnityEngine;

public class RedSpotInteractable : IInteractable
{
    public override float InteractRange => m_InteractRange;

    private float m_InteractRange = 0.15f;

    private HandJointInteractor m_Target;

    private float m_Speed = 1.5f;

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
        if (m_Target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, m_Target.transform.position, m_Speed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, m_Target.transform.position);
        if (m_Target.Interactable == null && dist < InteractRange)
            m_Target.Interactable = this;

        if (m_Target.Interactable == this && dist > InteractRange)
            m_Target.Interactable = null;
    }

    public override void Interact()
    {
        Debug.Log($"Triggered red spot");
    }
}
