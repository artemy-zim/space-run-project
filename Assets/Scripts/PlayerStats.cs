using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int _playerScore;
    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    private void Awake()
    {
        if (Instance != null)
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

    public int ScoreAdd(int amount)
    {
        PlayerScore += amount;
        return PlayerScore;

    }
}
