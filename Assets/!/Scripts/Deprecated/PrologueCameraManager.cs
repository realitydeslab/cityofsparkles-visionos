using Unity.XR.CoreUtils;
using UnityEngine;

public class PrologueCameraManager : MonoBehaviour
{
    private bool m_Registered = false;

    private XROrigin m_XROrigin;

    private void Update()
    {
        if (m_Registered)
            return;

        m_XROrigin = FindObjectOfType<XROrigin>();
        if (m_XROrigin != null)
        {
            m_XROrigin.gameObject.AddComponent<OrbitCamera>();
            m_Registered = true;
        }
    }

    private void OnDestroy()
    {
        if (m_XROrigin != null)
        {
            var orbitCamera = m_XROrigin.gameObject.GetComponent<OrbitCamera>();
            if (orbitCamera != null)
                Destroy(orbitCamera);
        }
    }
}
