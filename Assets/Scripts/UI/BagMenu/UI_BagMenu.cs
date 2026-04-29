using MoreMountains.Feedbacks;
using PrimeTween;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagMenu : UI_Menu
{
    [Header("Bag menu")]
    [SerializeField] private List<UI_BagSlot> _choiceSlotList;
    [SerializeField] private Button _confirm;
    [SerializeField] private Button _continue;

    public UI_BarValue _scoreBar;
    public UI_TextValue _currentScoreText;

    [Header("Round score")]
    [SerializeField] private Transform _roundScoreParent;
    [SerializeField] private UI_TextValue _roundScoreText;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _confettiLeft;
    [SerializeField] private ParticleSystem _confettiRight;
    [SerializeField] private MMF_Player _shakePlayer;

    [Header("Combinations")]
    [SerializeField] private List<UI_Combination> _combinationList = new();

    [Header("Bonus")]
    [SerializeField] private List<UI_BonusIcon> _bonusIconList = new();

    [Header("Time speed")]
    [SerializeField] private Transform _speedArrow;
    [SerializeField] private Vector3 _speedArrowOffset;

    private int _animationStackedNumber;
    private float _animationTimeScale = 1;

    private PrimeTween.Sequence _countSequence;
    int _roundScore = 0;
    private List<CombinationData> _combinationDataList = new();
    private List<BonusData> _bonusDataList = new();

    public override void OpenMenu()
    {
        _animationTimeScale = 1;

        _confirm.gameObject.SetActive(true);
        _continue.gameObject.SetActive(false);

        base.OpenMenu();

        _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal, false);
        _currentScoreText.SetTextValue($"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}", false);
        _roundScoreText.SetTextValue($"{0}", false);
        _roundScoreParent.gameObject.SetActive(false);

        List<ItemInstance> bagItemList = GameManager.Instance.ScavengingState.GetItemInstanceList();
        GameManager.Instance.ScavengingState.CleanItemDataList();
        GameManager.Instance.ScavengingState.UpdateItemNumberText();

        int size = GameManager.Instance.GetDepotSize();
        for (int i = 0; i < _choiceSlotList.Count; i++)
        {
            _choiceSlotList[i].ClearSlot();

            if (i < size)
            {
                _choiceSlotList[i].gameObject.SetActive(true);
            }
            else
            {
                _choiceSlotList[i].gameObject.SetActive(false);
            }

            if (i < bagItemList.Count)
            {
                _choiceSlotList[i].CreateItem(bagItemList[i]);
            }
        }

        for (int i = 0; i < _combinationList.Count; i++)
        {
            _combinationList[i].Reset();
        }

        for (int i = 0; i < _bonusIconList.Count; i++)
        {
            if (i < SaveManager.Instance.CurrentRunBonusList.Count)
            {
                _bonusIconList[i].Setup(SaveManager.Instance.CurrentRunBonusList[i]);
            }
            else
            {
                _bonusIconList[i].gameObject.SetActive(false);
            }
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

    public void Confirm()
    {
        CountScore();
        _confirm.gameObject.SetActive(false);
    }

    public void Continue()
    {
        GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState);
        _continue.gameObject.SetActive(false);
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

    public void CountScore()
    {
        _countSequence.Stop();
        _countSequence = PrimeTween.Sequence.Create();

        _roundScore = 0;
        float baseDelay = 0.5f;
        int totalAnimation = 0;

        _roundScoreText.SetTextValue($"{_roundScore}", false);
        _roundScoreParent.gameObject.SetActive(true);

        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();

        int countTimeMax = 1;
        BD_LastTurn_DoubleItem doubleTrouble = SaveManager.Instance.CheckHasRunBonus<BD_LastTurn_DoubleItem>();
        if (GameManager.Instance.CurrentRound >= GameManager.Instance.GetMaxRoundNumber() && doubleTrouble != null)
        {
            countTimeMax = 2;
        }
        for (int countTime = 0; countTime < countTimeMax; countTime++)
        {
            if (countTime == 1)
            {
                totalAnimation++;
                _countSequence.ChainDelay(baseDelay * _animationTimeScale);
                _countSequence.ChainCallback(() =>
                {
                    _shakePlayer.PlayFeedbacks();
                    StackAnim();
                    HighlightBonus(doubleTrouble.Name);
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText(doubleTrouble.Name, Vector3.up, Color.black, UI_TextPopper.AnimSpeed.Quick);
                });
                _countSequence.ChainDelay(.1f * _animationTimeScale);
            }
            ShowBaseScores();

            CountCombinations();

            CountItemBonus();
        }

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            int index = i;

            _countSequence.ChainDelay(.6f);
            _countSequence.ChainCallback(() =>
            {
                StackAnim();
                chosenItemSlotList[index].CurrentBagItem.CountItem();
            });
            _countSequence.ChainDelay(.2f);
            _countSequence.ChainCallback(() =>
            {
                _shakePlayer.PlayFeedbacks();
                _roundScore += chosenItemSlotList[index].CurrentBagItem.CurrentScore;
                _roundScoreText.SetTextValueNumber(_roundScore - chosenItemSlotList[index].CurrentBagItem.CurrentScore, _roundScore, .4f);
            });

            _countSequence.ChainDelay(.3f * _animationTimeScale);
            StackAnim();
        }

        CountTotalScoreBonus();

        _countSequence.ChainDelay(.8f * _animationTimeScale);
        _countSequence.ChainCallback(() =>
        {
            GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + _roundScore);
            SaveManager.CurrentSave.TotalPoints += _roundScore;
            QuestManager.Instance.CheckQuestCompletionByType<QD_TotalPoints>();
            _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);

            int confettiNumber = 0;
            if (_roundScore > 0) confettiNumber++;
            if (_roundScore > SaveManager.Instance.GetScavengeNode().ScoreGoal / 2) confettiNumber++;
            if (_roundScore > SaveManager.Instance.GetScavengeNode().ScoreGoal) confettiNumber++;
            _confettiLeft.Emit(confettiNumber * 50);
            _confettiRight.Emit(confettiNumber * 50);
            
            _currentScoreText.SetTextValue($"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}");
            _roundScoreParent.gameObject.SetActive(false);
            //chosenItemSlotList[index].HidePrice();
        });
        _countSequence.ChainDelay(1f);

        // End of round
        if (GameManager.Instance.CurrentRound >= GameManager.Instance.GetMaxRoundNumber())
        {
            _countSequence.ChainCallback(() =>
            {
                GameManager.Instance.CheckScoreHighEnough();
                GameManager.Instance.CurrentRound = 0;
            });
        }
        // Next hand
        else
        {
            _countSequence.ChainCallback(() =>
            {
                if (GameManager.Instance.CurrentScore >= SaveManager.Instance.GetScavengeNode().ScoreGoal)
                {
                    GameManager.Instance.SetGameState(GameManager.Instance.WinState);
                    GameManager.Instance.CurrentRound = 0;
                }
                else
                {
                    _continue.gameObject.SetActive(true);
                }
            });
        }

        _animationTimeScale = 5.0f / (float)totalAnimation;
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

    public void StackAnim()
    {
        _animationStackedNumber++;
        if (_animationStackedNumber > 1)
        {
            _animationStackedNumber = 0;
            _animationTimeScale *= .7f;
        }
    }

    public void ShowBaseScores()
    {
        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            UI_BagSlot bagSlot = chosenItemSlotList[i];
            if (bagSlot.CurrentBagItem.ItemInstance.Data.Price > 0) // if not garbage
            {
                _countSequence.ChainDelay(.5f);
                _countSequence.ChainCallback(() =>
                {
                    _shakePlayer.PlayFeedbacks();
                    bagSlot.CurrentBagItem.CurrentScore = bagSlot.CurrentBagItem.ItemInstance.CalculateValue();
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bagSlot.CurrentBagItem.CurrentScore, bagSlot.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    bagSlot.SetPriceText(bagSlot.CurrentBagItem.CurrentScore);
                    bagSlot.CurrentBagItem.CountBaseScore();
                });
            }
        }
        _countSequence.ChainDelay(1f);
    }

    public void CountCombinations()
    {
        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();
        List<CombinationData> combinationList = DataLoader.Instance.CombinationDataDictionary.Values.ToList();
        _combinationDataList.Clear();
        _bonusDataList.Clear();

        // calculate all addition per item combinations
        List<CombinationData> combinationItemAddList = combinationList.FindAll(x => x.Effect == CombinationData.CombinationEffect.ItemAddition);
        for (int i = 0; i < combinationItemAddList.Count; i++)
        {
            int index = i;
            List<UI_BagSlot> refChosenItemSlotList = new(chosenItemSlotList);
            if (combinationItemAddList[index].CheckCombination(ref refChosenItemSlotList))
            {
                int combinationNumber = _combinationDataList.Count;
                _combinationDataList.Add(combinationItemAddList[index]);
                _countSequence.ChainCallback(() =>
                {
                    _combinationList[combinationNumber].Setup(combinationItemAddList[index]);
                    _shakePlayer.PlayFeedbacks();
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{combinationItemAddList[index].Data.Name}", Vector3.up, Color.black);
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    UI_BagSlot bagSlot = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        int addBonus = combinationItemAddList[index].Bonus;
                        bagSlot.CurrentBagItem.CurrentScore += addBonus;
                        _shakePlayer.PlayFeedbacks();
                        bagSlot.SetPriceTextNumber(bagSlot.CurrentBagItem.CurrentScore - addBonus, bagSlot.CurrentBagItem.CurrentScore);
                        bagSlot.CurrentBagItem.CountBaseScore();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + addBonus, bagSlot.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }

        // calculate all multiplier per item combinations
        List<CombinationData> combinationItemMultList = combinationList.FindAll(x => x.Effect == CombinationData.CombinationEffect.ItemMultiplication);
        for (int i = 0; i < combinationItemMultList.Count; i++)
        {
            int index = i;
            List<UI_BagSlot> refChosenItemSlotList = new(chosenItemSlotList);
            if (combinationItemMultList[index].CheckCombination(ref refChosenItemSlotList))
            {
                int combinationNumber = _combinationDataList.Count;
                _combinationDataList.Add(combinationItemMultList[index]);
                _countSequence.ChainCallback(() =>
                {
                    _combinationList[combinationNumber].Setup(combinationItemMultList[index]);
                    _shakePlayer.PlayFeedbacks();
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{combinationItemMultList[index].Data.Name}", Vector3.up, Color.black);
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    UI_BagSlot bagSlot = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        int multBonus = combinationItemMultList[index].Bonus;
                        bagSlot.CurrentBagItem.CurrentScore *= multBonus;
                        _shakePlayer.PlayFeedbacks();
                        bagSlot.SetPriceTextNumber(bagSlot.CurrentBagItem.CurrentScore / multBonus, bagSlot.CurrentBagItem.CurrentScore);
                        bagSlot.CurrentBagItem.CountBaseScore();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + multBonus, bagSlot.transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }
    }

    public void CountItemBonus()
    {
        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();
        List<BonusData> runBonusList = SaveManager.Instance.CurrentRunBonusList;

        // calculate all addition to item score
        List<BonusData> bonusItemAddList = runBonusList.FindAll(x => x.Effect == BonusData.BonusEffect.ItemAddition);
        for (int i = 0; i < bonusItemAddList.Count; i++)
        {
            int index = i;
            List<UI_BagSlot> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusItemAddList[index].CheckBonus(ref refChosenItemSlotList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusItemAddList[index].Name);
                    //_shakePlayer.PlayFeedbacks();
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{bonusItemAddList[index].Name}", Vector3.up, Color.black);
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    UI_BagSlot bagSlot = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        int addBonus = Mathf.RoundToInt(bonusItemAddList[index].BonusValue);
                        bagSlot.CurrentBagItem.CurrentScore += addBonus;
                        _shakePlayer.PlayFeedbacks();
                        bagSlot.SetPriceTextNumber(bagSlot.CurrentBagItem.CurrentScore - addBonus, bagSlot.CurrentBagItem.CurrentScore);
                        bagSlot.CurrentBagItem.CountBaseScore();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + addBonus, bagSlot.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }

        // calculate all multiplication to item score
        List<BonusData> bonusItemMultList = runBonusList.FindAll(x => x.Effect == BonusData.BonusEffect.ItemMultiplication);
        for (int i = 0; i < bonusItemMultList.Count; i++)
        {
            int index = i;
            List<UI_BagSlot> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusItemMultList[index].CheckBonus(ref refChosenItemSlotList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusItemMultList[index].Name);
                    //_shakePlayer.PlayFeedbacks();
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{bonusItemMultList[index].Name}", Vector3.up, Color.black);
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    UI_BagSlot bagSlot = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        float multBonus = bonusItemMultList[index].BonusValue;
                        int formerScore = bagSlot.CurrentBagItem.CurrentScore;
                        bagSlot.CurrentBagItem.CurrentScore = Mathf.RoundToInt(multBonus * formerScore);
                        bagSlot.SetPriceTextNumber(bagSlot.CurrentBagItem.CurrentScore - formerScore, bagSlot.CurrentBagItem.CurrentScore);
                        bagSlot.CurrentBagItem.CountBaseScore();
                        _shakePlayer.PlayFeedbacks();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + multBonus, bagSlot.transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }
    }

    public void CountTotalScoreBonus()
    {
        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();
        List<BonusData> runBonusList = SaveManager.Instance.CurrentRunBonusList;

        // calculate all addition to total score
        List<BonusData> bonusTotalAddList = runBonusList.FindAll(x => x.Effect == BonusData.BonusEffect.TotalAddition);
        for (int i = 0; i < bonusTotalAddList.Count; i++)
        {
            int index = i;
            List<UI_BagSlot> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusTotalAddList[index].CheckBonus(ref refChosenItemSlotList, _combinationDataList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusTotalAddList[index].Name);
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{bonusTotalAddList[index].Name}", Vector3.up, Color.black);
                    _shakePlayer.PlayFeedbacks();
                });
                _countSequence.ChainDelay(.7f);
                _countSequence.ChainCallback(() =>
                {
                    int addBonus = Mathf.RoundToInt(bonusTotalAddList[index].BonusValue);
                    _shakePlayer.PlayFeedbacks();
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + addBonus, _roundScoreText.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    _roundScore += addBonus;
                    _roundScoreText.SetTextValueNumber(_roundScore - addBonus, _roundScore, .4f);
                });
                _countSequence.ChainDelay(1f);
            }
        }

        // calculate all addition per item combinations
        List<BonusData> bonusTotalMultList = runBonusList.FindAll(x => x.Effect == BonusData.BonusEffect.TotalMultiplication);
        for (int i = 0; i < bonusTotalMultList.Count; i++)
        {
            int index = i;
            List<UI_BagSlot> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusTotalMultList[index].CheckBonus(ref refChosenItemSlotList, _combinationDataList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusTotalMultList[index].Name);
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{bonusTotalMultList[index].Name}", Vector3.up, Color.black);
                    _shakePlayer.PlayFeedbacks();
                });
                _countSequence.ChainDelay(.7f);
                _countSequence.ChainCallback(() =>
                {
                    float multBonus = bonusTotalMultList[index].BonusValue;
                    _shakePlayer.PlayFeedbacks();
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + multBonus, _roundScoreText.transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    int formerScore = _roundScore;
                    _roundScore = Mathf.RoundToInt(multBonus * formerScore);
                    _roundScoreText.SetTextValueNumber(_roundScore - formerScore, _roundScore, .4f);
                });
                _countSequence.ChainDelay(1f);
            }
        }
    }

    public void HighlightBonus(string bonusName)
    {
        UI_BonusIcon bonusIcon = _bonusIconList.Find(x => x.Data.Name == bonusName);
        if (bonusIcon != null) bonusIcon.Highlight();
    }
}
