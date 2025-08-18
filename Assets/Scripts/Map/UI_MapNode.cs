using UnityEngine;

public class UI_MapNode : MonoBehaviour
{
    public MapNodeData MapNodeData;

    public void ChooseMapNode()
    {
        MapManager.Instance.LaunchNode(MapNodeData);
    }
}
