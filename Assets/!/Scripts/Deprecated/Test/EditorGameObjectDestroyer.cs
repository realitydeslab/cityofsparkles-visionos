using UnityEngine;

public class EditorGameObjectDestroyer : MonoBehaviour
{
    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif
    }
}
