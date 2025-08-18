using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    #region Singleton
    public static MapManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] private UI_MapNode _mapNodePrefab;

    public void GenerateMap()
    {

    }

    public void LaunchNode(MapNodeData data)
    {
        SaveManager.Instance.CurrentMapNode = data;
        SceneManager.LoadScene("Game");
    }
}
