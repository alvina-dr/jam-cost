using EasyTransition;
using PrimeTween;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private string _endingDialog;
    [SerializeField] private TransitionSettings _transitionSettings;

    [Header("Exclamation Mark")]
    [SerializeField] private GameObject _exclamationMark;
    [SerializeField] private float _exclamationMarkSpawnTime;
    [SerializeField] private float _exclamationMarkDuration;

    [Header("Triple Dot")]
    [SerializeField] private GameObject _tripleDot;
    [SerializeField] private float _tripleDotSpawnTime;
    [SerializeField] private float _tripleDotDuration;

    [Header("Double Exclamation Mark")]
    [SerializeField] private GameObject _doubleExclamationMark;
    [SerializeField] private float _doubleExclamationMarkSpawnTime;
    [SerializeField] private float _doubleExclamationMarkDuration;
    
    
    [SerializeField] private List<Sprite> _spriteList = new();
    [SerializeField] private List<Sprite> _markSpriteList = new();
    [SerializeField] private List<string> _stringList = new();
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _markSprite;
    [SerializeField] private TextMeshProUGUI _line;

    [SerializeField] private SpriteRenderer _blinkingTriangle;

    private int _index = 0;
    private bool _authorizeInput = false;

    private void Start()
    {
        //DialogueManager.Instance.DialogueRunner.StartDialogue(_endingDialog);
        DialogueManager.Instance.EndDialogueEvent += CallEndDialogueEvent;
        SaveManager.CurrentSave.NumberFirstBossWon++;
        QuestDirector.Instance.CheckQuestCompletionByType<QD_FirstBossNumber>();

        Tween.Delay(4, () =>
        {
            _authorizeInput = true;
            _blinkingTriangle.gameObject.SetActive(true);
        });

        //Sequence exclamationMarkSequence = Sequence.Create();
        //exclamationMarkSequence.ChainDelay(_exclamationMarkSpawnTime);
        //exclamationMarkSequence.ChainCallback(() => _exclamationMark.gameObject.SetActive(true));
        //exclamationMarkSequence.ChainDelay(_exclamationMarkDuration);
        //exclamationMarkSequence.ChainCallback(() => _exclamationMark.gameObject.SetActive(false));

        //Sequence tripleDotSequence = Sequence.Create();
        //tripleDotSequence.ChainDelay(_tripleDotSpawnTime);
        //tripleDotSequence.ChainCallback(() => _tripleDot.gameObject.SetActive(true));
        //tripleDotSequence.ChainDelay(_tripleDotDuration);
        //tripleDotSequence.ChainCallback(() => _tripleDot.gameObject.SetActive(false));

        //Sequence doubleExclamationMarkSequence = Sequence.Create();
        //doubleExclamationMarkSequence.ChainDelay(_doubleExclamationMarkSpawnTime);
        //doubleExclamationMarkSequence.ChainCallback(() => _doubleExclamationMark.gameObject.SetActive(true));
        //doubleExclamationMarkSequence.ChainDelay(_doubleExclamationMarkDuration);
        //doubleExclamationMarkSequence.ChainCallback(() => _doubleExclamationMark.gameObject.SetActive(false));
    }

    public void NextFrame()
    {
        if (_index == _spriteList.Count)
        {
            SaveManager.Instance.SaveRun();
            SaveManager.Instance.ChangeScene("Hub", _transitionSettings, 0);
            return;
        }
        _sprite.sprite = _spriteList[_index];
        _markSprite.sprite = _markSpriteList[_index];
        _line.SetText(_stringList[_index]);
        _index++;
    }

    private void Update()
    {
        if (!_authorizeInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            NextFrame();
        }
    }

    public void CallEndDialogueEvent()
    {
        SaveManager.Instance.AddMT(3);
        SaveManager.Instance.SaveRun();
        SaveManager.Instance.ChangeScene("Hub", _transitionSettings, 0);
    }

}
