using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    #region Singleton
    public static ClockManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] private MapData _mapData;
    [SerializeField] private ClockRoomIcon _clockRoomIconPrefab;

    [Header("Choice")]
    [SerializeField] private Transform _circleCenter;
    [SerializeField] private float _degreeStart;
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _degreeTotal;
    [SerializeField] private MapNode _choicePrefab;
    [SerializeField] private Transform _choiceParent;
    [SerializeField] private List<MapNode> _choiceList = new();


    public void Setup()
    {
        List<MapNodeData> chosenMapNodeData = new();

        MapNodeChoiceData choiceData = _mapData.ChoiceList[SaveManager.CurrentSave.CurrentRun.CurrentNode];
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

        for (int i = 0; i < chosenMapNodeData.Count; i++)
        {
            MapNode choice = Instantiate(_choicePrefab, _choiceParent);
            _choiceList.Add(choice);
            choice.Setup(chosenMapNodeData[i], choiceData.ChooseRandomReward());
            float zRotation = chosenMapNodeData.Count * _degreeTotal + _degreeStart;
            float x = _circleCenter.position.x + _circleRadius * Mathf.Cos((zRotation + 90) * Mathf.PI / 180);
            float y = _circleCenter.position.y + _circleRadius * Mathf.Sin((zRotation + 90) * Mathf.PI / 180);
            choice.transform.position = new Vector3(x, y, 0);
            // choose position depending on total number and degree of circle
        }
    }

    [Button]
    public void TestSystem(int choiceNumber)
    {
        for (int i = 0; i < _choiceList.Count; i++)
        {
            DestroyImmediate(_choiceList[i].gameObject);
        }

        _choiceList.Clear();

        float degreeSpace = _degreeTotal / (float) (choiceNumber - 1);
        for (int i = 0; i < choiceNumber; i++)
        {
            MapNode choice = Instantiate(_choicePrefab, _choiceParent);
            _choiceList.Add(choice);
            float zRotation = i * degreeSpace;
            float x = _circleCenter.position.x + _circleRadius * Mathf.Cos((zRotation + _degreeStart) * Mathf.PI / 180);
            float y = _circleCenter.position.y + _circleRadius * Mathf.Sin((zRotation + _degreeStart) * Mathf.PI / 180);
            choice.transform.position = new Vector3(x, y, 0);
            // choose position depending on total number and degree of circle
        }
    }
}
