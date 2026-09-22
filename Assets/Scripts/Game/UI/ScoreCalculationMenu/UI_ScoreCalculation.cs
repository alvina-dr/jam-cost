using MoreMountains.Feedbacks;
using PrimeTween;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreCalculation : UI_Menu
{
    [Header("Bag menu")]
    [SerializeField] private List<UI_BagSlot> _choiceSlotList;

    public Button ConfirmButton;
    public Button ContinueButton;

    [Header("Round score")]
    public Transform RoundScoreParent;
    public UI_TextValue RoundScoreText;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _confettiLeft;
    [SerializeField] private ParticleSystem _confettiRight;
    [SerializeField] private MMF_Player _shakePlayer;

    [Header("Combinations")]
    public List<UI_Combination> CombinationList = new();

    [Header("Time speed")]
    [SerializeField] private Transform _speedArrow;
    [SerializeField] private Vector3 _speedArrowOffset;

    private PrimeTween.Sequence _countSequence;
    private List<CombinationData> _combinationDataList = new();
    private List<BonusData> _bonusDataList = new();

    public override void OpenMenu()
    {
        ConfirmButton.gameObject.SetActive(true);
        ContinueButton.gameObject.SetActive(false);

        base.OpenMenu();

        RoundScoreText.SetTextValue($"{0}", false);
        RoundScoreParent.gameObject.SetActive(false);

        for (int i = 0; i < CombinationList.Count; i++)
        {
            CombinationList[i].Reset();
        }
    }

    private void Update()
    {
        if (!_isOpen)
        {
            Time.timeScale = 1.0f;
            return;
        }

        if (Input.GetMouseButton(0) && _countSequence.isAlive)
        {
            Time.timeScale = 2f;
            _speedArrow.gameObject.SetActive(true);
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.Instance.UIManager.Canvas.transform as RectTransform, Input.mousePosition + _speedArrowOffset, GameManager.Instance.UIManager.Canvas.worldCamera, out pos);
            _speedArrow.transform.position = GameManager.Instance.UIManager.Canvas.transform.TransformPoint(pos);
        }
        else
        {
            _speedArrow.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        SaveManager.Instance.GetScavengeNode().ExitScoreCount();
        ContinueButton.gameObject.SetActive(false);
    }

    public List<UI_BagSlot> GetChosenItemSlotList()
    {
        List<UI_BagSlot> chosenItemSlotList = new();
        for (int i = 0; i < _choiceSlotList.Count; i++)
        {
            if (_choiceSlotList[i].CurrentBagItem != null) chosenItemSlotList.Add(_choiceSlotList[i]);
        }
        return chosenItemSlotList;
    }

    public void ShakeList(List<UI_BagSlot> slotList)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            Sequence anim = Sequence.Create();
            anim.Chain(Tween.LocalPositionY(slotList[i].CurrentBagItem.transform, 20, .3f));
            anim.Chain(Tween.LocalPositionY(slotList[i].CurrentBagItem.transform, 0, .3f));
        }
    }

    public void AllowContinue()
    {
        ContinueButton.gameObject.SetActive(true);
    }

    public void Confirm()
    {
        ScoreCalculationManager.Instance.CountScore();
        ConfirmButton.gameObject.SetActive(false);
    }
}
