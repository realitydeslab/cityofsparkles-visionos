using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    [SerializeField] private string m_BaseScene;

    [SerializeField] private string m_CityScene;

    private AsyncOperation m_CitySceneLoadOperation;

    private void Start()
    {
        SceneManager.LoadScene(m_BaseScene);

        //m_CitySceneLoadOperation = SceneManager.LoadSceneAsync(m_CityScene, LoadSceneMode.Additive);
        //m_CitySceneLoadOperation.allowSceneActivation = true;
    }

    private void Update()
    {
        //if (m_CitySceneLoadOperation.isDone)
        //    SceneManager.UnloadSceneAsync("GameLauncher");
    }
}
