using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject[] MainMenuObjects;
    public bool IsActive = true;

    public static MainMenu Instance;

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

    public void ChangeMainMenuState()
    {
        IsActive = !IsActive;

        foreach(GameObject obj in MainMenuObjects)
        {
            obj.SetActive(IsActive);
        }
    }
}
