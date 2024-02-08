using System.Collections;
using UnityEngine;

public class SceneEngine : MonoBehaviour
{
    public static SceneEngine Instance;

    private const float _startingWorldSpeed = 5.0f;
    private const short _zeroWorldSpeed = 0;
    private bool _isGameRunning = false;

    public bool IsGameRunning
    {
        get { return _isGameRunning; }
        set { _isGameRunning = value; }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        ResumeGame();
    }

    public void InitGameStart()
    {
        IsGameRunning = true;

        WorldController.Instance.speed = _startingWorldSpeed;
        PlayerController.Instance.RunStart();
        MainMenu.Instance.ChangeMainMenuState();
        GameUI.Instance.ChangeGameUIState();
        StartCoroutine(OnScoreIncrement());
    }

    public void InitGamePauseOrContinue()
    {
        IsGameRunning = !IsGameRunning;

        if (!IsGameRunning)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

        PauseMenu.Instance.ChangePauseMenuState();
        GameUI.Instance.ChangeGameUIState();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void InitGameEnd()
    {
        IsGameRunning = false;
        StopCoroutine(OnScoreIncrement());

        WorldController.Instance.speed = _zeroWorldSpeed;
        GameUI.Instance.ChangeGameUIState();
        DeathMenu.Instance.ChangeDeathMenuState();
        SoundManager.Instance.StopMainThemeSound();
        SoundManager.Instance.PlayHitSound();
    }

    public void CollectGem(int score)
    {
        SoundManager.Instance.PlayCollectSound();
        PlayerStats.Instance.ScoreAdd(score);
    }

    private IEnumerator OnScoreIncrement()
    {
        while (Instance.IsGameRunning)
        {
            PlayerStats.Instance.ScoreAdd(1);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
