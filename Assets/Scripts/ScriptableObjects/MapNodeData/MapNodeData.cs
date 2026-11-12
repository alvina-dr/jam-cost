using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MapNodeData", menuName = "Scriptable Objects/MapNodeData")]
public class MapNodeData : ScriptableObject
{
    public SceneAsset Scene;
    public string NodeName;
    public bool IsAvailableInGame;
    public Sprite NodeIcon;
}
