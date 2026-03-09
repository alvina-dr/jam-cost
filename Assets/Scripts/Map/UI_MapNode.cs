using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MapNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MapNodeData MapNodeData;
    public int MapNodeIndex;
    public int MapNodeColumnIndex;
    public int MapNodeRowIndex;
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private UI_MapLine _lineRendererPrefab;
    [SerializeField] private List<int> _previousNodeList = new();
    public List<int> NextNodeList = new();
    [SerializeField] private List<UI_MapLine> _mapLineList = new();
    [SerializeField] private UI_Button _uIbutton;

    [Header("offset size for corners")]
    [SerializeField] private Vector2 _cornerOffset;

    public void SetupNode(MapNodeData nodeData, int nodeIndex, int columnIndex, int rowIndex)
    {
        SetupNode(nodeData);
        MapNodeIndex = nodeIndex;
        MapNodeRowIndex = rowIndex;
        MapNodeColumnIndex = columnIndex;
        transform.name = "MapNode_" + MapNodeIndex;
    }

    public void SetupNode(MapNodeData nodeData)
    {
        MapNodeData = nodeData;
        _icon.sprite = nodeData.NodeIcon;
        _icon.SetNativeSize();
    }

    public void SetupLine(UI_MapNode neighbourMapNode, bool xThenY)
    {
        if (_previousNodeList.Contains(neighbourMapNode.MapNodeIndex)) return;

        UI_MapLine mapLine = Instantiate(_lineRendererPrefab, MapManager.Instance.LineParent);
        mapLine.StartMapNode = neighbourMapNode;
        mapLine.EndMapNode = this;

        mapLine.transform.position = transform.position;
        Vector2 endPosition = neighbourMapNode.transform.position - transform.position;

        // if both nodes are on the same line
        if (endPosition.y != 0)
        {
            if (xThenY) mapLine.LineRenderer.points[1] = new Vector2(endPosition.x + _cornerOffset.x, 0);
            else mapLine.LineRenderer.points[1] = new Vector2(0, endPosition.y + (endPosition.y < 0 ? _cornerOffset.y : -_cornerOffset.y));

            if (xThenY) mapLine.LineRenderer.points[2] = new Vector2(endPosition.x, (endPosition.y < 0 ?- _cornerOffset.y : _cornerOffset.y));
            else mapLine.LineRenderer.points[2] = new Vector2(-_cornerOffset.x, endPosition.y);
        }

        mapLine.LineRenderer.points[3] = endPosition;

        _mapLineList.Add(mapLine);

        if (SaveManager.CurrentSave.CurrentRun.FormerNodeList.FindAll(x => x == neighbourMapNode.MapNodeIndex).Count > 0)
        {
            mapLine.LineRenderer.color = Color.red;
        }
        
        _previousNodeList.Add(neighbourMapNode.MapNodeIndex);
        neighbourMapNode.NextNodeList.Add(MapNodeIndex);
    }

    public void ChooseMapNode()
    {
        SaveManager.CurrentSave.CurrentRun.FormerNodeList.Add(MapNodeIndex);
        NodeChoiceManager.Instance.LaunchNode(MapNodeData);
    }

    public void DeactivateNode()
    {
        _uIbutton.enabled = false;
        _button.interactable = false;
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, .25f);
        SetWhiteLine();
    }

    public void ShowNodeAlreadyUsed()
    {
        _uIbutton.enabled = false;
        _button.interactable = false;
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, .5f);
    }

    public void ShowFormerNode()
    {
        _uIbutton.enabled = true;
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, 1f);
    }

    public bool AccessibleThroughNode(UI_MapNode startMapNode)
    {
        return (_previousNodeList.FindAll(x => x == startMapNode.MapNodeIndex).Count > 0);
    }

    public void SetWhiteLine()
    {
        for (int i = 0; i < _mapLineList.Count; i++)
        {
            _mapLineList[i].LineRenderer.color = Color.white;
        }
    }

    public void ShowPathToHere()
    {

    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_uIbutton.enabled) return;

        UI_MapLine mapLine = _mapLineList.Find(x => x.StartMapNode == MapManager.Instance.GetLastNode());
        if (mapLine != null)
        {
            mapLine.LineRenderer.color = Color.red;
            mapLine.transform.SetAsLastSibling();
        }
        // show path line
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_uIbutton.enabled) return;

        UI_MapLine mapLine = _mapLineList.Find(x => x.StartMapNode == MapManager.Instance.GetLastNode());
        if (mapLine != null)
        {
            mapLine.LineRenderer.color = Color.white;
        }
        // hide path line
    }
}
