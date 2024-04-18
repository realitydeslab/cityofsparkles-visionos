using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    [SerializeField] private float m_Height = 15000f;

    [SerializeField] private float m_Radius = 10000f;

    [SerializeField] private float m_Speed = 1.5f;

    private float m_Angle = 0f;

    private void Update()
    {
        if (m_Target == null)
            m_Target = GameObject.Find("City Center")?.transform;

        if (m_Target == null)
            return;

        m_Angle += m_Speed * Time.deltaTime;
        m_Angle %= 360f;

        float x = Mathf.Sin(Mathf.Deg2Rad * m_Angle) * m_Radius;
        float z = Mathf.Cos(Mathf.Deg2Rad * m_Angle) * m_Radius;

        transform.position = new(x + m_Target.position.x, m_Height, z + m_Target.position.z);
        transform.LookAt(m_Target);
    }

    private void OnDestroy()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
