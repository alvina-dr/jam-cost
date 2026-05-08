using UnityEngine;

[CreateAssetMenu(fileName = "MapNodeData", menuName = "Scriptable Objects/MapNodeData")]
public class MapNodeData : ScriptableObject
{
    public string NodeName;
    public bool IsAvailableInGame;
    public Sprite NodeIcon;
}
