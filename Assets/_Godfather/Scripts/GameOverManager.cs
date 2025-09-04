using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _gameOverTimeText;
    [SerializeField] private string _mainMenuSceneName = "Menu"; // Name of the main menu scene
    
    
    public UnityEvent OnGameOver;
    public void SetUpGameOver()
    {
        _gameOverUI.SetActive(true);
        _gameOverTimeText.text = "You protected the planet for " + _timer.GetTimeString();
        Time.timeScale = 0f; // Pause the game
        OnGameOver?.Invoke();
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Resume the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(_mainMenuSceneName);
    }

}
