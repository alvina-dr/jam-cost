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
    [SerializeField] private Transform _currentMapNodeIcon;

    [SerializeField] private List<UI_MapNode> _mapNodeList;
    [SerializeField] private List<int> _nodeNumberPerColumn;
    public UI_MapNode StartingMapNode;
    public UI_MapNode EndingMapNode;
    [SerializeField] private Vector2 _offsetRandomLimit;
    [SerializeField] private Vector2 _interNodeSpace;
    [SerializeField] private Vector3 _offsetGeneralMap;

    [Header("Tiles")]
    [SerializeField] private Transform _tileParent;
    [SerializeField] private Image _tile1x1Building;

    private List<int> _formerNodeList => SaveManager.CurrentSave.CurrentRun.FormerNodeList;

    public void OnAwake()
    {
        _dayText.SetTextValue((SaveManager.CurrentSave.CurrentRun.CurrentNode + 1).ToString());

        if (_formerNodeList.Count == 0)
        {
            SaveManager.CurrentSave.CurrentRun.RandomSeed = (int)System.DateTime.Now.Ticks;
            _formerNodeList.Add(StartingMapNode.MapNodeIndex);
        }

        //_mapNodeList.Add(_startingMapNode);
        //_nodeNumberPerColumn.Add(1);

        Random.InitState(SaveManager.CurrentSave.CurrentRun.RandomSeed);

        // instantiate all nodes
        for (int i = 0; i < _mapData.DailyChoiceList.Count; i++)
        {
            _nodeNumberPerColumn.Add(0);
            for (int j = 0; j < _mapData.DailyChoiceList[i].MapNodeDataList.Count; j++)
            {
                _nodeNumberPerColumn[i] += 1;
                UI_MapNode mapNode = Instantiate(_mapNodePrefab, _mapNodeParent);

                int mapNodeIndex = i * _mapData.DailyChoiceList[0].MapNodeDataList.Count + j + 1; // + 1 because of the starting map node
                mapNode.transform.localPosition = new Vector3(i * _interNodeSpace.x, j * _interNodeSpace.y, 0) + 
                    new Vector3(Random.Range(-_offsetRandomLimit.x, _offsetRandomLimit.x), Random.Range(-_offsetRandomLimit.y, _offsetRandomLimit.y), 0)
                    + _offsetGeneralMap;
                mapNode.SetupNode(_mapData.DailyChoiceList[i].MapNodeDataList[j], _mapNodeList.Count, i, j);
                _mapNodeList.Add(mapNode);
            }
        }

        RemoveNodes();

        ConnectNodes();

        SetNodesState();

        AddTiles();

        _currentMapNodeIcon.transform.position = GetLastNode().transform.position;
        _currentMapNodeIcon.transform.position += new Vector3(0, 75);
    }

    public void LaunchNode(MapNodeData data)
    {
        SaveManager.Instance.CurrentMapNode = data;
        switch (data) 
        {
            case MND_Scavenge_Classic:
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
        for (int i = 0; i < 1; i++)
        {
            RemoveRandomNode();
        }
    }

    public void RemoveRandomNode()
    {
        int column = Random.Range(0, _nodeNumberPerColumn.Count);
        //List<UI_MapNode> columnNodeList = GetNodeListFromColumn(column);

        for (int i = 0; i < _nodeNumberPerColumn.Count; i++)
        {
            List<UI_MapNode> columnNodeListTest = GetNodeListFromColumn(i);

            if (columnNodeListTest.Count > 1)
            {
                Debug.Log("deactivate node on column : " + i);
                columnNodeListTest[Random.Range(0, columnNodeListTest.Count)].gameObject.SetActive(false);
            }
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
                _mapNodeList[i].SetupLine(StartingMapNode, true);
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
            EndingMapNode.SetupLine(lastColumnNodeList[i], false);
        }
    }

    public void SetNodesState()
    {
        UI_MapNode lastMapNode = StartingMapNode;
        if (_formerNodeList.Count > 1
            && _formerNodeList[^1] != -1)
        {
            lastMapNode = _mapNodeList[_formerNodeList[^1]];
        }

        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            // if node is not of today column 
            if (_mapNodeList[i].MapNodeColumnIndex != SaveManager.CurrentSave.CurrentRun.CurrentNode) 
            {
                // if node is not part of previous nodes
                if (!_formerNodeList.Contains(_mapNodeList[i].MapNodeIndex))
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
                if (SaveManager.CurrentSave.CurrentRun.CurrentNode != 0)
                {
                    if (_formerNodeList.Last() != -1)
                    {
                        if (!_mapNodeList[_formerNodeList.Last()].NextNodeList.Contains(_mapNodeList[i].MapNodeIndex))
                        {
                            _mapNodeList[i].DeactivateNode();
                        }
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

    public UI_MapNode GetLastNode()
    {
        if (_formerNodeList.Count > 1)
        {
            return _mapNodeList.Find(x => x.MapNodeIndex == _formerNodeList.Last());
        }
        else
        {
            return StartingMapNode;
        }
    }
}
