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

    [SerializeField] private ClockRoomIcon _clockRoomIconPrefab;

    [Header("Choice")]
    [SerializeField] private SpriteRenderer _twoChoices;
    [SerializeField] private SpriteRenderer _threeChoices;
    [SerializeField] private SpriteRenderer _fourChoices;
    [SerializeField] private SpriteRenderer _fiveChoices;
    [SerializeField] private Transform _choiceCircleCenter;
    [SerializeField] private float _degreeStart;
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _degreeTotal;
    [SerializeField] private MapNode _choicePrefab;
    [SerializeField] private Transform _choiceParent;
    [SerializeField] private List<MapNode> _choiceList = new();

    [Header("Rooms")]
    [SerializeField] private ClockRoomIcon _roomPrefab;
    [SerializeField] private Transform _roomCircleCenter;
    [SerializeField] private Transform _roomParent;
    [SerializeField] private float _roomDegreeStart;
    [SerializeField] private float _roomCircleRadius;
    [SerializeField] private float _roomDegreeTotal;

    [Header("Clock Hands")]
    [SerializeField] private Transform _bigHand;
    [SerializeField] private Transform _smallHand;

    public void Setup()
    {
        List<MapNodeData> chosenMapNodeData = new();

        MapNodeChoiceData choiceData = NodeChoiceManager.Instance.MapData.ChoiceList[SaveManager.CurrentSave.CurrentRun.CurrentNode];
        List<MapNodeData> choiceList = new(choiceData.MapNodeDataPool);

        int numberNodeToDraw = 3;

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

        for (int i = 0; i < _choiceList.Count; i++)
        {
            DestroyImmediate(_choiceList[i].gameObject);
        }

        _choiceList.Clear();

        float degreeSpace = _degreeTotal / (float)(chosenMapNodeData.Count - 1);
        for (int i = 0; i < chosenMapNodeData.Count; i++)
        {
            MapNode choice = Instantiate(_choicePrefab, _choiceParent);
            _choiceList.Add(choice);
            choice.Setup(chosenMapNodeData[i], choiceData.ChooseRandomReward());
            float zRotation = i * degreeSpace;
            float x = _choiceCircleCenter.position.x + _circleRadius * Mathf.Cos((zRotation + _degreeStart) * Mathf.PI / 180);
            float y = _choiceCircleCenter.position.y + _circleRadius * Mathf.Sin((zRotation + _degreeStart) * Mathf.PI / 180);
            choice.transform.position = new Vector3(x, y, 0);
        }

        _twoChoices.gameObject.SetActive(false);
        _threeChoices.gameObject.SetActive(false);
        _fourChoices.gameObject.SetActive(false);
        _fiveChoices.gameObject.SetActive(false);
        switch (chosenMapNodeData.Count)
        {
            case 1:
            case 2:
                _twoChoices.gameObject.SetActive(true);
                break;
            case 3:
                _threeChoices.gameObject.SetActive(true);
                break;
            case 4:
                _fourChoices.gameObject.SetActive(true);
                break;
            case 5:
                _fiveChoices.gameObject.SetActive(true);
                break;
        }

        float roomDegreeSpace = _roomDegreeTotal / (float)(NodeChoiceManager.Instance.MapData.ChoiceList.Count - 1);
        for (int i = 0; i < NodeChoiceManager.Instance.MapData.ChoiceList.Count; i++)
        {
            ClockRoomIcon roomIcon = Instantiate(_roomPrefab, _roomParent);
            float zRotation = i * roomDegreeSpace;
            float x = _choiceCircleCenter.position.x + _roomCircleRadius * Mathf.Cos((zRotation + _roomDegreeStart) * Mathf.PI / 180);
            float y = _choiceCircleCenter.position.y + _roomCircleRadius * Mathf.Sin((zRotation + _roomDegreeStart) * Mathf.PI / 180);
            roomIcon.transform.position = new Vector3(x, y, 0);
            if (i < SaveManager.CurrentSave.CurrentRun.CurrentNode) roomIcon.Enable();
            else roomIcon.Disable();
        }
    }

    private void Update()
    {
        Vector2 direction =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - _bigHand.transform.position;
        if (direction.y < 0) direction = new Vector2(direction.x, 0);
        _bigHand.transform.up = direction.normalized;
    }

    [Button]
    public void SetupClock(int choiceNumber)
    {
        for (int i = 0; i < _choiceList.Count; i++)
        {
            DestroyImmediate(_choiceList[i].gameObject);
        }

        _choiceList.Clear();

        float degreeSpace = _roomDegreeTotal / (float) (choiceNumber - 1);
        for (int i = 0; i < choiceNumber; i++)
        {
            MapNode choice = Instantiate(_choicePrefab, _choiceParent);
            _choiceList.Add(choice);
            float zRotation = i * degreeSpace;
            float x = _choiceCircleCenter.position.x + _circleRadius * Mathf.Cos((zRotation + _degreeStart) * Mathf.PI / 180);
            float y = _choiceCircleCenter.position.y + _circleRadius * Mathf.Sin((zRotation + _degreeStart) * Mathf.PI / 180);
            choice.transform.position = new Vector3(x, y, 0);
            // choose position depending on total number and degree of circle
        }
    }
}
