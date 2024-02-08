using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;

    public GameObject[] GameUIObjects;
    public static GameUI Instance;

    private bool _isActive = true;

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

    private void Start()
    {
        ChangeGameUIState();
        ScoreUpdateText(0);
    }

    private void Update()
    {
        ScoreUpdateText(PlayerStats.Instance.PlayerScore);
    }

    public void ChangeGameUIState()
    {
        _isActive = !_isActive;

        foreach (GameObject obj in GameUIObjects)
        {
            obj.SetActive(_isActive);
        }
    }

    public void ScoreUpdateText(int score)
    {
        _tmpText.text = $"Ñ÷¸ò: {score}";
    }
}
