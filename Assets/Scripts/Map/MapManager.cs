using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            OnAwake();
        }
    }
    #endregion

    [SerializeField] private UI_MapNode _mapNodePrefab;
    [SerializeField] private Transform _mapNodeParent;
    public Transform LineParent;
    [SerializeField] private MapData _mapData;
    [SerializeField] private UI_TextValue _dayText;
    [SerializeField] private Transform _racoonIcon;

    [SerializeField] private List<UI_MapNode> _mapNodeList;
    [SerializeField] private List<int> _nodeNumberPerColumn;
    [SerializeField] private UI_MapNode _startingMapNode;
    [SerializeField] private UI_MapNode _endingMapNode;
    [SerializeField] private Vector2 _offsetRandomLimit;

    [Header("Tiles")]
    [SerializeField] private Transform _tileParent;
    [SerializeField] private Image _tile1x1Building;

    public void OnAwake()
    {
        _dayText.SetTextValue((SaveManager.Instance.CurrentSave.CurrentDay + 1).ToString());

        if (SaveManager.Instance.CurrentSave.CurrentDay == 0)
        {
            SaveManager.Instance.CurrentSave.RandomSeed = (int)System.DateTime.Now.Ticks;
            SaveManager.Instance.CurrentSave.FormerNodeList.Add(_startingMapNode.MapNodeIndex);
        }

        //_mapNodeList.Add(_startingMapNode);
        //_nodeNumberPerColumn.Add(1);

        Random.InitState(SaveManager.Instance.CurrentSave.RandomSeed);

        // instantiate all nodes
        for (int i = 0; i < _mapData.DailyChoiceList.Count; i++)
        {
            _nodeNumberPerColumn.Add(0);
            for (int j = 0; j < _mapData.DailyChoiceList[i].MapNodeDataList.Count; j++)
            {
                _nodeNumberPerColumn[i] += 1;
                UI_MapNode mapNode = Instantiate(_mapNodePrefab, _mapNodeParent);

                int mapNodeIndex = i * _mapData.DailyChoiceList[0].MapNodeDataList.Count + j + 1; // + 1 because of the starting map node
                mapNode.transform.localPosition = new Vector3(i * 200f, j * 200f, 0) + 
                    new Vector3(Random.Range(-_offsetRandomLimit.x, _offsetRandomLimit.x), Random.Range(-_offsetRandomLimit.y, _offsetRandomLimit.y), 0);
                mapNode.SetupNode(_mapData.DailyChoiceList[i].MapNodeDataList[j], _mapNodeList.Count, i, j);
                _mapNodeList.Add(mapNode);
            }
        }

        RemoveNodes();

        ConnectNodes();

        SetNodesState();

        AddTiles();

        if (SaveManager.Instance.CurrentSave.FormerNodeList.Count > 1)
        {
            _racoonIcon.transform.position = _mapNodeList.Find(x => x.MapNodeIndex == SaveManager.Instance.CurrentSave.FormerNodeList.Last()).transform.position;
            _racoonIcon.transform.position += new Vector3(0, 100);
        }
        else
        {
            _racoonIcon.transform.position = _startingMapNode.transform.position;
            _racoonIcon.transform.position += new Vector3(0, 100);
        }
    }

    public void LaunchNode(MapNodeData data)
    {
        SaveManager.Instance.CurrentMapNode = data;
        switch (data) 
        {
            case MND_ClassicScavenge:
                SceneManager.LoadScene("Game");
                break;
            case MND_NPC:
                SceneManager.LoadScene("Shop");
                break;
            case MND_Shop:
                SceneManager.LoadScene("Shop");
                break;
        }
    }

    public void RemoveNodes()
    {
        for (int i = 0; i < 3; i++)
        {
            RemoveRandomNode();
        }
    }

    public void RemoveRandomNode()
    {
        int column = Random.Range(0, _nodeNumberPerColumn.Count);
        List<UI_MapNode> columnNodeList = GetNodeListFromColumn(column);
        
        if (columnNodeList.Count <= 1)
        {
            RemoveRandomNode();
        }
        else
        {
            columnNodeList[Random.Range(0, columnNodeList.Count)].gameObject.SetActive(false);
        }
    }

    public void ConnectNodes()
    {
        // CONNECT NODES
        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            // skip deactivated nodes
            if (!_mapNodeList[i].gameObject.activeSelf) { }
            // on first column connect all nodes to starting node
            else if (_mapNodeList[i].MapNodeColumnIndex == 0)
            {
                _mapNodeList[i].SetupLine(_startingMapNode, true);
            }
            // we get all nodes from next column and pick random ones to connect with
            else if (_mapNodeList[i].MapNodeColumnIndex < _nodeNumberPerColumn.Count)
            {
                UI_MapNode leftNode = GetNodeAtCoordinates(_mapNodeList[i].MapNodeRowIndex, _mapNodeList[i].MapNodeColumnIndex - 1);

                //Check if there is a node on the next column of the next row
                if (leftNode != null && leftNode.gameObject.activeSelf)
                {
                    _mapNodeList[i].SetupLine(leftNode, true);
                }
                else
                {
                    List<UI_MapNode> columnNodeList = GetNodeListFromColumn(_mapNodeList[i].MapNodeColumnIndex - 1);
                    UI_MapNode closestNode = columnNodeList.Aggregate((x, y) => System.Math.Abs(x.MapNodeRowIndex - _mapNodeList[i].MapNodeRowIndex) < System.Math.Abs(y.MapNodeRowIndex - _mapNodeList[i].MapNodeRowIndex) ? x : y);
                    _mapNodeList[i].SetupLine(closestNode, true);
                }
            }
        }

        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            if (_mapNodeList[i].gameObject.activeSelf && _mapNodeList[i].NextNodeList.Count == 0)
            {
                List<UI_MapNode> columnNodeList = GetNodeListFromColumn(_mapNodeList[i].MapNodeColumnIndex + 1);
                if (columnNodeList.Count > 0)
                {
                    UI_MapNode closestNode = columnNodeList.Aggregate((x, y) => System.Math.Abs(x.MapNodeRowIndex - _mapNodeList[i].MapNodeRowIndex) < System.Math.Abs(y.MapNodeRowIndex - _mapNodeList[i].MapNodeRowIndex) ? x : y);
                    closestNode.SetupLine(_mapNodeList[i], false);
                }
            }
        }

        // Connect ending node to all nodes of last column
        List<UI_MapNode> lastColumnNodeList = GetNodeListFromColumn(_mapData.DailyChoiceList.Count - 1);
        for (int i = 0; i < lastColumnNodeList.Count; i++)
        {
            _endingMapNode.SetupLine(lastColumnNodeList[i], false);
        }
    }

    public void SetNodesState()
    {
        UI_MapNode lastMapNode = _startingMapNode;
        if (SaveManager.Instance.CurrentSave.FormerNodeList.Count > 1) lastMapNode = _mapNodeList[SaveManager.Instance.CurrentSave.FormerNodeList.Last()];

        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            // if node is not of today column 
            if (_mapNodeList[i].MapNodeColumnIndex != SaveManager.Instance.CurrentSave.CurrentDay) 
            {
                // if node is not part of previous nodes
                if (!SaveManager.Instance.CurrentSave.FormerNodeList.Contains(_mapNodeList[i].MapNodeIndex))
                {
                    _mapNodeList[i].DeactivateNode();
                }
                else
                {
                    _mapNodeList[i].ShowNodeAlreadyUsed();
                }
            }
            else
            {
                _mapNodeList[i].SetWhiteLine();
                if (SaveManager.Instance.CurrentSave.CurrentDay != 0)
                {
                    if (!_mapNodeList[SaveManager.Instance.CurrentSave.FormerNodeList.Last()].NextNodeList.Contains(_mapNodeList[i].MapNodeIndex))
                    {
                        _mapNodeList[i].DeactivateNode();
                    }
                }
            }
        }
    }

    public void AddTiles()
    {
        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            UI_MapNode currentMapNode = _mapNodeList[i];
            if (_mapNodeList[i].gameObject.activeSelf)
            {
                List<UI_MapNode> nextColumnNodeList = GetNodeListFromColumn(currentMapNode.MapNodeColumnIndex + 1);

                for (int j = 0; j < nextColumnNodeList.Count; j++)
                {
                    if (nextColumnNodeList[j].MapNodeRowIndex == currentMapNode.MapNodeRowIndex + 1
                        || nextColumnNodeList[j].MapNodeRowIndex == currentMapNode.MapNodeRowIndex - 1)
                    {
                        UI_MapNode diagonalMapNode = _mapNodeList[nextColumnNodeList[j].MapNodeIndex];
                        Image tile = Instantiate(_tile1x1Building, _tileParent);
                        tile.transform.position = new Vector3((diagonalMapNode.transform.position.x + currentMapNode.transform.position.x)/2f,
                            (diagonalMapNode.transform.position.y + currentMapNode.transform.position.y)/2f, 0);
                        tile.name = _tile1x1Building.name + "_" + currentMapNode.MapNodeIndex + "-" + diagonalMapNode.MapNodeIndex;
                    }
                }
            }
        }
    }

    public UI_MapNode GetNodeAtCoordinates(int row, int column)
    {
        return _mapNodeList.Find(mapNode => mapNode.MapNodeRowIndex == row && mapNode.MapNodeColumnIndex == column);
    }

    public List<UI_MapNode> GetNodeListFromColumn(int columnIndex, bool includeInactive = false)
    {
        if (includeInactive) return _mapNodeList.FindAll(x => x.MapNodeColumnIndex == columnIndex);
        return _mapNodeList.FindAll(x => x.MapNodeColumnIndex == columnIndex && x.gameObject.activeSelf);
    }
}
