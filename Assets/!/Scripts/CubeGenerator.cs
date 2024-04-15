using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_CubePrefab;

    [SerializeField] private int m_Row = 20;

    [SerializeField] private int m_Column = 20;

    [SerializeField] private float m_IntervalDist = 1f;

    private void Start()
    {
        for (int i = -m_Row / 2; i < m_Row / 2; i++)
        {
            for (int j = -m_Column / 2; j < m_Column / 2; j++)
            {
                Vector3 spawnPosition = new(i, 0f, j);
                Instantiate(m_CubePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
