using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;

public class PlayerPoseIndicator : MonoBehaviour
{
    private TMP_Text m_PlayerPoseText;

    private XROrigin m_XROrigin;

    private void Start()
    {
        m_PlayerPoseText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (m_XROrigin == null)
        {
            m_XROrigin = FindObjectOfType<XROrigin>();
            if (m_XROrigin == null)
                return;
        }

        m_PlayerPoseText.text = $"Player Position: {m_XROrigin.transform.position:F2}\nPlayer Rotation: {m_XROrigin.transform.rotation.eulerAngles:F2}";
    }
}
