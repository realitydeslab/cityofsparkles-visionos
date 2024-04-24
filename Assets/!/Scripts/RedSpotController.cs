using UnityEngine;

public class RedSpotController : IInteractable
{
    public override float InteractRange => m_InteractRange;

    private float m_InteractRange = 0.15f;

    private Transform m_Target;

    private float m_Speed = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Target != null)
            return;

        if (other.TryGetComponent<HandJointInteractor>(out var handJoint))
        {
            if (handJoint.HandJointID != UnityEngine.XR.Hands.XRHandJointID.IndexTip)
                return;

            m_Target = handJoint.transform;
        }
    }

    private void Update()
    {
        if (m_Target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, m_Target.position, m_Speed * Time.deltaTime);
    }

    public override void Interact()
    {
        Debug.Log($"Triggered red spot");
    }
}
