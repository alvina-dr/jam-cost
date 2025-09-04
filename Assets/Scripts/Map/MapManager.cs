using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    [SerializeField] private UI_MapNode _startingMapNode;

    public void OnAwake()
    {
        _dayText.SetTextValue((SaveManager.Instance.CurrentSave.CurrentDay + 1).ToString());

        if (SaveManager.Instance.CurrentSave.CurrentDay == 0)
        {
            SaveManager.Instance.CurrentSave.RandomSeed = (int)System.DateTime.Now.Ticks;
            SaveManager.Instance.CurrentSave.FormerNodeList.Add(_startingMapNode.MapNodeIndex);
        }

        _mapNodeList.Add(_startingMapNode);

        Random.InitState(SaveManager.Instance.CurrentSave.RandomSeed);

        for (int i = 0; i < _mapData.DailyChoiceList.Count; i++)
        {
            for (int j = 0; j < _mapData.DailyChoiceList[i].MapNodeDataList.Count; j++)
            {
                UI_MapNode mapNode = Instantiate(_mapNodePrefab, _mapNodeParent);

                int mapNodeIndex = i * _mapData.DailyChoiceList[0].MapNodeDataList.Count + j + 1; // + 1 because of the starting map node
                mapNode.transform.position = new Vector3(i * 200f,
                    j * 200f, 0);
                _mapNodeList.Add(mapNode);
                mapNode.SetupNode(_mapData.DailyChoiceList[i].MapNodeDataList[j], mapNodeIndex);

                mapNode.name = "MapNode_" + mapNodeIndex;

                if (i == 0) mapNode.SetupLine(_startingMapNode);
                else
                {
                    for (int k = 0; k < 2; k++)
                    {
                        int connectedIndex = Random.Range((i - 1) * _mapData.DailyChoiceList[0].MapNodeDataList.Count, i * _mapData.DailyChoiceList[0].MapNodeDataList.Count) + 1;
                        mapNode.SetupLine(_mapNodeList[connectedIndex]);
                    }
                }

                // For nodes from other days
                if (SaveManager.Instance.CurrentSave.CurrentDay != i)
                {
                    if (SaveManager.Instance.CurrentSave.FormerNodeList.FindAll(x => x == mapNodeIndex).Count == 0)
                        mapNode.DeactivateNode();
                }
                // For nodes from today
                else
                {
                    mapNode.SetWhiteLine();

                    // if it's NOT the first day
                    if (SaveManager.Instance.CurrentSave.CurrentDay != 0)
                    {
                        // if the node is NOT accessible by the last node
                        if (!mapNode.AccessibleThroughNode(_mapNodeList[SaveManager.Instance.CurrentSave.FormerNodeList.Last()]))
                        {
                            mapNode.DeactivateNode();
                        }
                    } 
                }

                //else if (SaveManager.Instance.CurrentSave.CurrentDay == 0 && ])) mapNode.DeactivateNode();

                if (i < SaveManager.Instance.CurrentSave.FormerNodeList.Count)
                {
                    if (mapNodeIndex == SaveManager.Instance.CurrentSave.FormerNodeList[i]) mapNode.ShowFormerNode();
                } 
            }
        }

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
        SceneManager.LoadScene("Game");
    }
}
