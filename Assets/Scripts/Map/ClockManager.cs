using PrimeTween;
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
    [SerializeField] private List<ClockRoomIcon> _roomIconList = new();

    [Header("Clock Hands")]
    [SerializeField] private Transform _bigHand;
    [SerializeField] private Transform _smallHand;
    [SerializeField] private float _handMoveSpeed;

    private bool _animationOnGoing;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        _animationOnGoing = true;
        List<MapNodeData> chosenMapNodeData = GetAllChosenNodes();
        MapNodeChoiceData choiceData = SaveManager.Instance.MapData.ChoiceList[SaveManager.CurrentSave.CurrentRun.CurrentNode];


        for (int i = 0; i < _choiceList.Count; i++)
        {
            DestroyImmediate(_choiceList[i].gameObject);
        }

        _choiceList.Clear();

        float degreeSpace = _degreeTotal / (float)(chosenMapNodeData.Count - 1);
        if (chosenMapNodeData.Count == 1)
        {
            degreeSpace = _degreeTotal;
            //NodeChoiceManager.Instance.LaunchNode(chosenMapNodeData[0], choiceData.ChooseRandomReward());
            //return;
        }

        int scavengeNodeCount = chosenMapNodeData.FindAll(x => x is MND_Scavenge_Classic).Count;
        List<RewardData> randomRewardList = choiceData.GetRandomRewardList(scavengeNodeCount);
        for (int i = 0; i < chosenMapNodeData.Count; i++)
        {
            MapNode choice = Instantiate(_choicePrefab, _choiceParent);
            _choiceList.Add(choice);
            if (chosenMapNodeData[i] is MND_Scavenge_Classic)
            {
                choice.Setup(chosenMapNodeData[i], randomRewardList[0]);
                randomRewardList.RemoveAt(0);
            }
            else
            {
                choice.Setup(chosenMapNodeData[i], null);
            }
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

        _bigHand.transform.up = Vector3.left;

        // ROOMS 
        Sequence roomIconSequence = Sequence.Create();
        roomIconSequence.ChainDelay(1f);
        float roomDegreeSpace = -_roomDegreeTotal / (float)(SaveManager.Instance.MapData.ChoiceList.Count - 1);
        for (int i = 0; i < SaveManager.Instance.MapData.ChoiceList.Count; i++)
        {
            ClockRoomIcon roomIcon = Instantiate(_roomPrefab, _roomParent);
            _roomIconList.Add(roomIcon);
            roomIcon.name = "Room Icon " + i;
            float zRotation = i * roomDegreeSpace + 180;
            float x = _choiceCircleCenter.position.x + _roomCircleRadius * Mathf.Cos((zRotation + -_roomDegreeStart) * Mathf.PI / 180);
            float y = _choiceCircleCenter.position.y + _roomCircleRadius * Mathf.Sin((zRotation + -_roomDegreeStart) * Mathf.PI / 180);
            roomIcon.transform.position = new Vector3(x, y, 0);

            roomIcon.Disable();
            if (i < SaveManager.CurrentSave.CurrentRun.CurrentNode)
            {
                roomIconSequence.ChainCallback(() => roomIcon.Enable());
                roomIconSequence.ChainDelay(.1f);
            }

            if (i == SaveManager.CurrentSave.CurrentRun.CurrentNode)
            {
                roomIconSequence.ChainCallback(() => roomIcon.Enable());
                roomIconSequence.ChainDelay(.3f);
                roomIconSequence.ChainCallback(() => roomIcon.Disable());
                roomIconSequence.ChainDelay(.3f);
                roomIconSequence.ChainCallback(() => roomIcon.Enable());
                roomIconSequence.ChainDelay(.3f);
                roomIconSequence.ChainCallback(() => roomIcon.Disable());
                roomIconSequence.ChainDelay(.3f);
                roomIconSequence.ChainCallback(() => roomIcon.Enable());
                roomIconSequence.ChainDelay(.3f);
            }
        }
        
        Vector2 formerDirection = _roomIconList[0].transform.position - _smallHand.transform.position;
        //Debug.DrawLine(_smallHand.transform.position, _roomIconList[0].transform.position, color:Color.red, 5);
        _smallHand.transform.up = formerDirection;

        Vector2 direction = formerDirection;

        if (SaveManager.CurrentSave.CurrentRun.CurrentNode > 0)
        {
            direction = _roomIconList[SaveManager.CurrentSave.CurrentRun.CurrentNode].transform.position - _smallHand.transform.position;
            //Debug.DrawLine(_smallHand.transform.position, _roomIconList[SaveManager.CurrentSave.CurrentRun.CurrentNode].transform.position, color:Color.blue, 5);

            float angle = Vector2.SignedAngle(Vector3.up, direction.normalized);

            //_smallHand.transform.eulerAngles = new Vector3(0, 0, angle);
            //Debug.Log("current node is : " + SaveManager.CurrentSave.CurrentRun.CurrentNode);
            //Debug.Log("point at " + _roomIconList[SaveManager.CurrentSave.CurrentRun.CurrentNode].name);
            
            roomIconSequence.Chain(Tween.Rotation(_smallHand, new Vector3(0, 0, angle), .6f));
            roomIconSequence.ChainCallback(() => _animationOnGoing = false);
        }
        else
        {
            roomIconSequence.ChainDelay(.6f);
            roomIconSequence.ChainCallback(() => _animationOnGoing = false);
            //_animationOnGoing = false;
        }
    }

    public static List<MapNodeData> GetAllChosenNodes()
    {
        List<MapNodeData> chosenMapNodeData = new();

        MapNodeChoiceData choiceData = SaveManager.Instance.MapData.ChoiceList[SaveManager.CurrentSave.CurrentRun.CurrentNode];
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

        return chosenMapNodeData;
    }

    private void Update()
    {
        if (_animationOnGoing) return;
        Vector2 direction =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - _bigHand.transform.position;
        if (direction.y < 0) direction = new Vector2(direction.x, 0);
        _bigHand.transform.up = Vector3.Lerp(_bigHand.transform.up, direction.normalized, Time.deltaTime * _handMoveSpeed);
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
