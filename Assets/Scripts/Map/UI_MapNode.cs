using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapNode : MonoBehaviour
{
    public MapNodeData MapNodeData;
    public int MapNodeIndex;
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private UI_LineRenderer _lineRendererPrefab;
    [SerializeField] private List<int> _previousNodeList = new();
    [SerializeField] private List<UI_LineRenderer> _lineRendererList = new();

    public void SetupNode(MapNodeData nodeData, int nodeIndex)
    {
        MapNodeData = nodeData;
        MapNodeIndex = nodeIndex;
        _icon.sprite = nodeData.NodeIcon;
        _icon.SetNativeSize();
    }

    public void SetupLine(UI_MapNode neighbourMapNode)
    {
        if (_previousNodeList.Contains(neighbourMapNode.MapNodeIndex)) return;

        UI_LineRenderer lineRenderer = Instantiate(_lineRendererPrefab, MapManager.Instance.LineParent);
        lineRenderer.transform.position = transform.position;
        lineRenderer.points[1] = neighbourMapNode.transform.position - transform.position;
        _lineRendererList.Add(lineRenderer);

        if (SaveManager.Instance.CurrentSave.FormerNodeList.FindAll(x => x == neighbourMapNode.MapNodeIndex).Count > 0)
        {
            lineRenderer.color = Color.red;
        }
        
        _previousNodeList.Add(neighbourMapNode.MapNodeIndex);
    }

    public void ChooseMapNode()
    {
        MapManager.Instance.LaunchNode(MapNodeData);
        SaveManager.Instance.CurrentSave.FormerNodeList.Add(MapNodeIndex);
    }

    public void DeactivateNode()
    {
        _button.interactable = false;
        _icon.color = Color.black;
        
        SetWhiteLine();
    }

    public void ShowFormerNode()
    {
        _icon.color = Color.white;
    }

    public bool AccessibleThroughNode(UI_MapNode startMapNode)
    {
        Debug.Log("accessible through last node : " + (_previousNodeList.FindAll(x => x == startMapNode.MapNodeIndex).Count > 0));
        return (_previousNodeList.FindAll(x => x == startMapNode.MapNodeIndex).Count > 0);
    }

    public void SetWhiteLine()
    {
        for (int i = 0; i < _lineRendererList.Count; i++)
        {
            _lineRendererList[i].color = Color.white;
        }
    }
}
