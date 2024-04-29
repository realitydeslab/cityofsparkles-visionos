using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularTest : MonoBehaviour
{
    [SerializeField] private Transform m_C;

    [SerializeField] private Transform m_A;

    [SerializeField] private float m_Angle;

    [SerializeField] private GameObject m_Prefab;

    private void Start()
    {
        SpawnBallOnCircle(45f);
        SpawnBallOnCircle(135f);
        SpawnBallOnCircle(180f);
    }

    private void SpawnBallOnCircle(float angle)
    {
        float radius = Vector3.Distance(m_C.position, m_A.position);
        float radians = angle * Mathf.PI / 180f;
        float relativeAngle = Mathf.Atan2(m_A.position.z - m_C.position.z, m_A.position.x - m_C.position.x);
        float finalAngle = relativeAngle - radians;

        Vector3 position = new(m_C.transform.position.x + radius * Mathf.Cos(finalAngle), 0f, m_C.transform.position.z + radius * Mathf.Sin(finalAngle));
        Instantiate(m_Prefab, position, Quaternion.identity);
    }
}
