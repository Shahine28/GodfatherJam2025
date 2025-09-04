using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}
