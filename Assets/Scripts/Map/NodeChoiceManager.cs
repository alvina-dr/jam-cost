using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeChoiceManager : MonoBehaviour
{
    #region Singleton
    public static NodeChoiceManager Instance { get; private set; }

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

    [SerializeField] private MapData _mapData;
    [SerializeField] private List<UI_MapNode> _mapNodeList = new();

    private void OnAwake()
    {
        if (SaveManager.CurrentSave.CurrentRun.CurrentNode == 0)
        {
            SaveManager.CurrentSave.CurrentRun.RandomSeed = (int)System.DateTime.Now.Ticks;
        }

        Random.InitState(SaveManager.CurrentSave.CurrentRun.RandomSeed);

        SetupChoice();
    }

    public void SetupChoice()
    {
        List<MapNodeData> chosenMapNodeData = new();

        if (SaveManager.CurrentSave.CurrentRun.CurrentNode == _mapData.BossNumber)
        {
            chosenMapNodeData.Add(_mapData.BossMapNodeData);
        }
        else
        {
            List<MapNodeData> choiceList = new(_mapData.DailyChoiceList[SaveManager.CurrentSave.CurrentRun.CurrentNode].MapNodeDataList);
            for (int i = 0; i < 2; i++)
            { 
                if (choiceList.Count == 0) break;
                MapNodeData mapNodeData = choiceList[Random.Range(0, choiceList.Count)];
                choiceList.Remove(mapNodeData);
                chosenMapNodeData.Add(mapNodeData);
            }
        }

        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            if (i < chosenMapNodeData.Count)
            {
                _mapNodeList[i].gameObject.SetActive(true);
                _mapNodeList[i].SetupNode(chosenMapNodeData[i]);
            }
            else
            {
                _mapNodeList[i].gameObject.SetActive(false);
            }
        }
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
            case MND_Boss:
                SceneManager.LoadScene("Ending");
                break;
        }
    }
}
