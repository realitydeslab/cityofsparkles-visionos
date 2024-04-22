using UnityEngine;
using Unity.XR.CoreUtils;

public class PlayerGeoController : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerStartPoint;

    private bool m_StartPointInitialized = false;

    private void Update()
    {
        if (!m_StartPointInitialized)
        {
            XROrigin xrOrigin = FindObjectOfType<XROrigin>();
            if (xrOrigin != null)
            {
                xrOrigin.transform.SetPositionAndRotation(m_PlayerStartPoint.position, m_PlayerStartPoint.rotation);
                m_StartPointInitialized = true;
            }
        }
    }
}
