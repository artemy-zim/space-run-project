using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneEngine.Instance.InitGameStart();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneEngine.Instance.IsGameRunning)
        {
            SceneEngine.Instance.InitGamePauseOrContinue();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !SceneEngine.Instance.IsGameRunning)
        {
            SceneEngine.Instance.InitGamePauseOrContinue();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (!MainMenu.Instance.IsActive && !DeathMenu.Instance.isActive)
        {
            HandleInput();
        }
    }
}
