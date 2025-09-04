using UnityEngine;

[CreateAssetMenu(fileName = "MapNodeData", menuName = "Scriptable Objects/MapNodeData")]
public class MapNodeData : ScriptableObject
{
    public bool IsAvailableInGame;
    public Sprite NodeIcon;
}
