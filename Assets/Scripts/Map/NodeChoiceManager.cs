using EasyTransition;
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

    public MapData MapData;
    [SerializeField] private List<UI_MapNode> _mapNodeList = new();
    [SerializeField] private TransitionSettings _transitionSettings;
    [SerializeField] private ClockManager _clockManager;

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

        MapNodeChoiceData choiceData = MapData.ChoiceList[SaveManager.CurrentSave.CurrentRun.CurrentNode];
        List<MapNodeData> choiceList = new(choiceData.MapNodeDataPool);

        int numberNodeToDraw = 2;

        MND_FreeRound freeRound = choiceList.Find(x => x is MND_FreeRound) as MND_FreeRound;
        if (freeRound != null && SaveManager.CurrentSave.CurrentRun.RunBonusRound == 0)
        {
            choiceList.Remove(freeRound);
            chosenMapNodeData.Add(freeRound);
            numberNodeToDraw--;
        }
        for (int i = 0; i < numberNodeToDraw; i++)
        { 
            if (choiceList.Count == 0) break;
            MapNodeData mapNodeData = choiceList[Random.Range(0, choiceList.Count)];
            choiceList.Remove(mapNodeData);
            chosenMapNodeData.Add(mapNodeData);
        }

        for (int i = 0; i < _mapNodeList.Count; i++)
        {
            if (i < chosenMapNodeData.Count)
            {
                _mapNodeList[i].gameObject.SetActive(true);
                _mapNodeList[i].SetupNode(chosenMapNodeData[i], choiceData.ChooseRandomReward());
            }
            else
            {
                _mapNodeList[i].gameObject.SetActive(false);
            }
        }

        _clockManager.Setup();
    }

    public void LaunchNode(MapNodeData mapNodeData, RewardData rewardData)
    {
        SaveManager.Instance.CurrentMapNode = Instantiate(mapNodeData);
        SaveManager.Instance.CurrentReward = rewardData;
        switch (mapNodeData)
        {
            case MND_Scavenge_Classic:
                SaveManager.Instance.ChangeScene("Game", _transitionSettings, 0);
                break;
            case MND_FreeRound:
                SaveManager.Instance.ChangeScene("FreeRound", _transitionSettings, 0);
                break;
            case MND_Shop:
                SaveManager.Instance.ChangeScene("Shop", _transitionSettings, 0);
                break;
            case MND_Boss:
                SaveManager.Instance.ChangeScene("Possession", _transitionSettings, 0);
                break;
        }
    }
}
