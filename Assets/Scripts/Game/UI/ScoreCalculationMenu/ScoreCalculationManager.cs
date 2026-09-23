using MoreMountains.Feedbacks;
using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScoreCalculationManager : MonoBehaviour
{
    #region Singleton
    public static ScoreCalculationManager Instance { get; private set; }

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

    [SerializeField] private SortingGroup _sortingGroup;

    [Header("Items")]
    [SerializeField] private List<Item_ScoreCalculation> _itemList = new();
    [SerializeField] private Item_ScoreCalculation _itemPrefab;
    [SerializeField] private float _itemSpace;
    [SerializeField] private float _itemSize;
    [SerializeField] private Transform _itemParent;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _confettiLeft;
    [SerializeField] private ParticleSystem _confettiRight;
    [SerializeField] private MMF_Player _shakePlayer;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;

    private int _animationStackedNumber;
    private float _animationTimeScale = 1;

    private PrimeTween.Sequence _countSequence;
    private List<CombinationData> _combinationDataList = new();
    private List<BonusData> _bonusDataList = new();
    int _roundScore = 0;

    public void Show()
    {
        Setup();

        _sortingGroup.sortingLayerName = "ScoreCalculation";
        Tween.LocalPositionY(transform, 0, .5f).OnComplete(() =>
        {
            GameManager.Instance.ItemManager.HideItems();
            GameManager.Instance.CrateOverCheck.gameObject.SetActive(false);
            GameManager.Instance.DepotOverCheck.gameObject.SetActive(false);
        });

        BonusHandManager.Instance.Show();

        _animationTimeScale = 1;

        GameManager.Instance.UIManager.BagMenu.OpenMenu();

        GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore, false);
        GameManager.Instance.UIManager.ScoreTextValue.SetTextValue($"{GameManager.Instance.CurrentScore} / {GameManager.Instance.GoalScore}", false);

        List<ItemInstance> bagItemList = GameManager.Instance.ScavengingState.GetItemInstanceList();
        GameManager.Instance.ScavengingState.CleanItemDataList();
        GameManager.Instance.ScavengingState.UpdateItemNumberText();

        Tween.Delay(1f, () => CountScore());
    }

    public void Setup()
    {
        List<ItemInstance> bagItemList = GameManager.Instance.ScavengingState.GetItemInstanceList();

        int size = GameManager.Instance.GetDepotSize();

        for (int i = 0; i < _itemList.Count; i++)
        {
            if (i < bagItemList.Count)
            {
                _itemList[i].Setup(bagItemList[i]);
            }
            else
            {
                _itemList[i].gameObject.SetActive(false);
            }
        }

        List<Item_ScoreCalculation> activeItemList = _itemList.FindAll(x => x.gameObject.activeSelf);
        for (int i = 0; i < activeItemList.Count; i++)
        {
            float totalSpace = _itemSpace * (activeItemList.Count - 1) + _itemSize * activeItemList.Count;
            activeItemList[i].transform.localPosition = new Vector3((i * _itemSpace) + (i * _itemSize + _itemSize / 2) - totalSpace / 2, 0);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.BagState)
        {
            Time.timeScale = 1.0f;
            return;
        }

        if (Input.GetMouseButton(0) && _countSequence.isAlive)
        {
            Time.timeScale = 2f;
            GameManager.Instance.UIManager.BagMenu.SpeedArrow.gameObject.SetActive(true);
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.Instance.UIManager.Canvas.transform as RectTransform, Input.mousePosition + GameManager.Instance.UIManager.BagMenu.SpeedArrowOffset, GameManager.Instance.UIManager.Canvas.worldCamera, out pos);
            GameManager.Instance.UIManager.BagMenu.SpeedArrow.transform.position = GameManager.Instance.UIManager.Canvas.transform.TransformPoint(pos);
        }
        else
        {
            GameManager.Instance.UIManager.BagMenu.SpeedArrow.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Hide()
    {
        BonusHandManager.Instance.Hide();
        GameManager.Instance.ItemManager.ShowItems();
        GameManager.Instance.CrateOverCheck.gameObject.SetActive(true);
        GameManager.Instance.DepotOverCheck.gameObject.SetActive(true);

        Tween.LocalPositionY(transform, 10.8f, .5f).OnComplete(() =>
        {
            _sortingGroup.sortingLayerName = "Background";
        });
    }

    public void HighlightBonus(string bonusName)
    {
        BonusBehavior bonus = BonusHandManager.Instance.GetBonus(bonusName);
        if (bonus != null) bonus.Highlight();
    }

    public void AllowContinue()
    {
        GameManager.Instance.UIManager.BagMenu.ContinueButton.gameObject.SetActive(true);
    }

    #region Score Calculation

    public List<Item_ScoreCalculation> GetChosenItemList()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = _itemList.FindAll(x => x.gameObject.activeSelf);
        return chosenItemSlotList;
    }

    public void CountScore()
    {
        _countSequence.Stop();
        _countSequence = PrimeTween.Sequence.Create();

        _roundScore = 0;
        float baseDelay = 0.5f;
        int totalAnimation = 0;

        GameManager.Instance.UIManager.BagMenu.RoundScoreText.SetTextValue($"{_roundScore}", false);
        GameManager.Instance.UIManager.BagMenu.RoundScoreParent.gameObject.SetActive(true);

        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();

        ShowBaseScores();

        CountCombinations();

        CountItemBonus();

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            int index = i;

            _countSequence.ChainDelay(.6f);
            _countSequence.ChainCallback(() =>
            {
                StackAnim();
                chosenItemSlotList[index].CountItem();
            });
            _countSequence.ChainDelay(.2f);
            _countSequence.ChainCallback(() =>
            {
                CameraManager.Instance.SimpleShake();
                _roundScore += chosenItemSlotList[index].CurrentScore;
                GameManager.Instance.UIManager.BagMenu.RoundScoreText.SetTextValueNumber(_roundScore - chosenItemSlotList[index].CurrentScore, _roundScore, .4f);
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
            QuestDirector.Instance.CheckQuestCompletionByType<QD_TotalPoints>();
            GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore);

            int confettiNumber = 0;
            if (_roundScore > 0) confettiNumber++;
            if (_roundScore > GameManager.Instance.GoalScore / 2) confettiNumber++;
            if (_roundScore > GameManager.Instance.GoalScore) confettiNumber++;
            _confettiLeft.Emit(confettiNumber * 50);
            _confettiRight.Emit(confettiNumber * 50);

            GameManager.Instance.UIManager.ScoreTextValue.SetTextValue($"{GameManager.Instance.CurrentScore} / {GameManager.Instance.GoalScore}");
            GameManager.Instance.UIManager.BagMenu.RoundScoreParent.gameObject.SetActive(false);
            //chosenItemSlotList[index].HidePrice();
        });
        _countSequence.ChainDelay(1f);

        _countSequence.ChainCallback(() =>
        {
            CheckFinalScore();
        });

        _animationTimeScale = 5.0f / (float)totalAnimation;
    }

    public void ShowBaseScores()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            Item_ScoreCalculation item = chosenItemSlotList[i];
            if (item.ItemInstance.Data.Price > 0) // if not garbage
            {
                _countSequence.ChainDelay(.5f);
                _countSequence.ChainCallback(() =>
                {
                    CameraManager.Instance.SimpleShake();
                    item.CurrentScore = item.ItemInstance.CalculateValue();
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + item.CurrentScore, item.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    item.SetPriceText(item.CurrentScore);
                    item.CountBaseScore();
                });
            }
        }
        _countSequence.ChainDelay(1f);
    }

    public void CountCombinations()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();
        List<CombinationData> combinationList = DataLoader.Instance.CombinationDataDictionary.Values.ToList();
        _combinationDataList.Clear();
        _bonusDataList.Clear();

        // calculate all addition per item combinations
        List<CombinationData> combinationItemAddList = combinationList.FindAll(x => x.Effect == CombinationData.CombinationEffect.ItemAddition);
        for (int i = 0; i < combinationItemAddList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (combinationItemAddList[index].CheckCombination(ref refChosenItemSlotList))
            {
                int combinationNumber = _combinationDataList.Count;
                _combinationDataList.Add(combinationItemAddList[index]);
                _countSequence.ChainCallback(() =>
                {
                    GameManager.Instance.UIManager.BagMenu.CombinationList[combinationNumber].Setup(combinationItemAddList[index]);
                    CameraManager.Instance.SimpleShake();
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{combinationItemAddList[index].Data.Name}", Vector3.up, Color.black);
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    Item_ScoreCalculation bagSlot = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        int addBonus = combinationItemAddList[index].Bonus;
                        bagSlot.CurrentScore += addBonus;
                        CameraManager.Instance.SimpleShake();
                        bagSlot.SetPriceTextNumber(bagSlot.CurrentScore - addBonus, bagSlot.CurrentScore);
                        bagSlot.CountBaseScore();
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
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (combinationItemMultList[index].CheckCombination(ref refChosenItemSlotList))
            {
                int combinationNumber = _combinationDataList.Count;
                _combinationDataList.Add(combinationItemMultList[index]);
                _countSequence.ChainCallback(() =>
                {
                    GameManager.Instance.UIManager.BagMenu.CombinationList[combinationNumber].Setup(combinationItemMultList[index]);
                    CameraManager.Instance.SimpleShake();
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText($"<wave amp=2>{combinationItemMultList[index].Data.Name}", Vector3.up, Color.black);
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    Item_ScoreCalculation item = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        int multBonus = combinationItemMultList[index].Bonus;
                        item.CurrentScore *= multBonus;
                        CameraManager.Instance.SimpleShake();
                        item.SetPriceTextNumber(item.CurrentScore / multBonus, item.CurrentScore);
                        item.CountBaseScore();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + multBonus, item.transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }
    }

    public void CountItemBonus()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();
        List<BonusData> runBonusList = SaveManager.Instance.CurrentRunBonusList;

        // calculate all addition to item score
        List<BonusData> bonusItemAddList = runBonusList.FindAll(x => x.Effect == BonusEffect.ItemAddition);
        for (int i = 0; i < bonusItemAddList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusItemAddList[index].CheckBonus(ref refChosenItemSlotList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusItemAddList[index].Name);
                    //_shakePlayer.PlayFeedbacks();
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    Item_ScoreCalculation item = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        int addBonus = Mathf.RoundToInt(bonusItemAddList[index].BonusValue);
                        item.CurrentScore += addBonus;
                        CameraManager.Instance.SimpleShake();
                        item.SetPriceTextNumber(item.CurrentScore - addBonus, item.CurrentScore);
                        item.CountBaseScore();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + addBonus, item.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }

        // calculate all multiplication to item score
        List<BonusData> bonusItemMultList = runBonusList.FindAll(x => x.Effect == BonusEffect.ItemMultiplication);
        for (int i = 0; i < bonusItemMultList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusItemMultList[index].CheckBonus(ref refChosenItemSlotList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusItemMultList[index].Name);
                    //_shakePlayer.PlayFeedbacks();
                });
                _countSequence.ChainDelay(.2f);
                for (int j = 0; j < refChosenItemSlotList.Count; j++)
                {
                    Item_ScoreCalculation item = refChosenItemSlotList[j];
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        float multBonus = bonusItemMultList[index].BonusValue;
                        int formerScore = item.CurrentScore;
                        item.CurrentScore = Mathf.RoundToInt(multBonus * formerScore);
                        item.SetPriceTextNumber(item.CurrentScore - formerScore, item.CurrentScore);
                        item.CountBaseScore();
                        CameraManager.Instance.SimpleShake();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + multBonus, item.transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    });
                }
                _countSequence.ChainDelay(1f);
            }
        }
    }

    public void CountTotalScoreBonus()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();
        List<BonusData> runBonusList = SaveManager.Instance.CurrentRunBonusList;

        // calculate all addition to total score
        List<BonusData> bonusTotalAddList = runBonusList.FindAll(x => x.Effect == BonusEffect.TotalAddition);
        for (int i = 0; i < bonusTotalAddList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusTotalAddList[index].CheckBonus(ref refChosenItemSlotList, _combinationDataList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusTotalAddList[index].Name);
                    CameraManager.Instance.SimpleShake();
                });
                _countSequence.ChainDelay(.7f);
                _countSequence.ChainCallback(() =>
                {
                    int addBonus = Mathf.RoundToInt(bonusTotalAddList[index].BonusValue);
                    CameraManager.Instance.SimpleShake();
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + addBonus, GameManager.Instance.UIManager.BagMenu.RoundScoreText.transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    _roundScore += addBonus;
                    GameManager.Instance.UIManager.BagMenu.RoundScoreText.SetTextValueNumber(_roundScore - addBonus, _roundScore, .4f);
                });
                _countSequence.ChainDelay(1f);
            }
        }

        // calculate all addition per item combinations
        List<BonusData> bonusTotalMultList = runBonusList.FindAll(x => x.Effect == BonusEffect.TotalMultiplication);
        for (int i = 0; i < bonusTotalMultList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusTotalMultList[index].CheckBonus(ref refChosenItemSlotList, _combinationDataList))
            {
                _countSequence.ChainCallback(() =>
                {
                    HighlightBonus(bonusTotalMultList[index].Name);
                    CameraManager.Instance.SimpleShake();
                });
                _countSequence.ChainDelay(.7f);
                _countSequence.ChainCallback(() =>
                {
                    float multBonus = bonusTotalMultList[index].BonusValue;
                    CameraManager.Instance.SimpleShake();
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + multBonus, GameManager.Instance.UIManager.BagMenu.RoundScoreText.transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    int formerScore = _roundScore;
                    _roundScore = Mathf.RoundToInt(multBonus * formerScore);
                    GameManager.Instance.UIManager.BagMenu.RoundScoreText.SetTextValueNumber(_roundScore - formerScore, _roundScore, .4f);
                });
                _countSequence.ChainDelay(1f);
            }
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

    public void CheckFinalScore()
    {
        MND_Scavenge_Classic scavengeNode = SaveManager.Instance.GetScavengeNode();
        if (GameManager.Instance.CurrentScore >= GameManager.Instance.GoalScore)
        {
            scavengeNode.Victory();
        }
        else
        {
            if (GameManager.Instance.CurrentRound >= GameManager.Instance.GetMaxRoundNumber())
            {
                scavengeNode.Defeat();
            }
            else
            {
                AllowContinue();
            }
        }
    }
    #endregion

#if UNITY_EDITOR
    [Button]
    public void Instantiate(int number)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            DestroyImmediate(_itemList[i].gameObject);
        }

        _itemList.Clear();

        for (int i = 0; i < number; i++)
        {
            Item_ScoreCalculation bonusBehavior = PrefabUtility.InstantiatePrefab(_itemPrefab, _itemParent) as Item_ScoreCalculation;
            float totalSpace = _itemSpace * (number - 1) + _itemSize * number;
            bonusBehavior.transform.localPosition = new Vector3((i * _itemSpace) + (i * _itemSize + _itemSize / 2) - totalSpace / 2, 0);
            _itemList.Add(bonusBehavior);
        }
    }
#endif
}
