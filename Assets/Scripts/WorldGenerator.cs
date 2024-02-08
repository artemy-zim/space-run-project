using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private float collectibleDistFromPlatformByY = 1.0f;

    private float _pathInterDist;
    public GameObject[] CollectibleObjects;

    public GameObject[] FreePlatforms;
    public GameObject[] ObstaclePlatforms;
    public Transform PlatformContainer;

    private bool _isObstacle;

    private Transform _lastPlatform = null;

    void Start()
    {
        _pathInterDist = PlayerController.Instance.RollDistance;

        Init();
    }

    public void Init()
    {
        CreateFreePlatform();
        CreateFreePlatform();

        for (int i = 0; i < 12; i++)
        {
            CreateCollectible();
            CreatePlatform();
        }
    }

    public void CreatePlatform()
    {
        if (_isObstacle)
            CreateFreePlatform();
        else
            CreateObstaclePlatform();
    }

    public void CreateCollectible()
    {
        if (!_isObstacle)
        {
            int index = Random.Range(0, CollectibleObjects.Length);
            int path = Random.Range(1, 4);

            float xPos;
            float yPos = _lastPlatform.position.y + collectibleDistFromPlatformByY;
            float zPos = _lastPlatform.GetComponent<MeshRenderer>().bounds.center.z;

            switch (path)
            {
                case 1:
                    xPos = _lastPlatform.position.x - _pathInterDist;
                    break;
                case 2:
                    xPos = _lastPlatform.position.x;
                    break;
                case 3:
                    xPos = _lastPlatform.position.x + _pathInterDist;
                    break;
                default:
                    xPos = 0f;
                    Debug.LogWarning("Ошибка X позиции для collectibles");
                    break;
            }
            Vector3 pos = new Vector3(xPos, yPos, zPos);

            Instantiate(CollectibleObjects[index], pos, Quaternion.identity, PlatformContainer);
            WorldController.Instance.collectiblesCount++;
        }
    }

    private void CreateFreePlatform()
    {
        Vector3 pos = (_lastPlatform == null) ?
            PlatformContainer.position : _lastPlatform.GetComponent<PlatformController>().endPoint.position;

        int index = Random.Range(0, FreePlatforms.Length);
        GameObject res = Instantiate(FreePlatforms[index], pos, Quaternion.identity, PlatformContainer);
        _lastPlatform = res.transform;

        _isObstacle = false;
    }

    private void CreateObstaclePlatform()
    {
        Vector3 pos = (_lastPlatform == null) ?
            PlatformContainer.position : _lastPlatform.GetComponent<PlatformController>().endPoint.position;

        int index = Random.Range(0, ObstaclePlatforms.Length);
        GameObject res = Instantiate(ObstaclePlatforms[index], pos, Quaternion.identity, PlatformContainer);
        _lastPlatform = res.transform;

        _isObstacle = true;
    }
}
