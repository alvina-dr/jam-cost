using MoreMountains.Feedbacks;
using PrimeTween;
using Sirenix.OdinInspector;
using System;
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

    private int _animationStackedNumber;
    private float _animationTimeScale = 1;

    private PrimeTween.Sequence _countSequence;
    private List<CombinationData> _combinationDataList = new();
    private List<BonusData> _bonusDataList = new();
    int _roundScore = 0;

    private float _baseScore;
    private float _multiplierScore;
    public CombinationData CurrentCombinationData;


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
        Tween.Delay(.7f, () => BonusHandManager.Instance.Show());

        _animationTimeScale = 1;

        GameManager.Instance.UIManager.BagMenu.OpenMenu();

        GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore, false);
        GameManager.Instance.UIManager.ScoreTextValue.SetTextValue($"{GameManager.Instance.CurrentScore} / {GameManager.Instance.GoalScore}", false);

        GameManager.Instance.ScavengingState.CleanSelectedItemList();
        GameManager.Instance.ScavengingState.UpdateItemNumberText();

        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();

        if (CurrentCombinationData != null)
        {
            GameManager.Instance.PreviousCombinationPlayed = CurrentCombinationData;
            if (GameManager.Instance.FirstCombinationPlayed == null) GameManager.Instance.FirstCombinationPlayed = CurrentCombinationData;
        }

        CurrentCombinationData = CheckCombination(ItemDirector.GetItemDataList(chosenItemSlotList));
        if (CurrentCombinationData != null)
        {
            _baseScore = CurrentCombinationData.BaseScore;
            _multiplierScore = CurrentCombinationData.MultiplierScore;
            GameManager.Instance.UIManager.BagMenu.CurrentCombinationInfo.Setup(CurrentCombinationData);
        }

        GameManager.Instance.UIManager.BagMenu.AddScoreText.SetTextValue($"{_baseScore}", false);
        GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.SetTextValue($"{_multiplierScore}", false);

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
            //Time.timeScale = 1.0f;
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
            if (Time.timeScale == 2f) Time.timeScale = 1f;
        }
    }

    public void Hide()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance.CurrentScore < GameManager.Instance.GoalScore)
        {
            BonusHandManager.Instance.Hide();
        }
        else if (SaveManager.Instance.CurrentReward.RewardItemData.Prefab is not CB_Bonus) BonusHandManager.Instance.Hide();
        
        GameManager.Instance.ItemManager.ShowItems();
        GameManager.Instance.CrateOverCheck.gameObject.SetActive(true);
        GameManager.Instance.DepotOverCheck.gameObject.SetActive(true);
        GameManager.Instance.UIManager.BagMenu.CloseMenu();

        Tween.LocalPositionY(transform, 10.8f, .5f).OnComplete(() =>
        {
            _sortingGroup.sortingLayerName = "Background";

        });
    }

    public void HighlightBonus(string bonusName)
    {
        BonusBehavior bonus = BonusHandManager.Instance.GetBonus(bonusName);
        AudioManager.PlaySound(AudioManager.Instance.BonusHighlight);
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

        CountItems();

        CheckFinalBonus();

        _countSequence.ChainDelay(1f);
        _countSequence.ChainCallback(() =>
        {
            _roundScore = Mathf.RoundToInt(_baseScore * _multiplierScore);
            GameManager.Instance.UIManager.BagMenu.RoundScoreText.SetTextValueNumber(0, _roundScore);
            GameManager.Instance.UIManager.BagMenu.AddScoreText.SetTextValue("0");
            GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.SetTextValue("0");
        });

        //CountTotalScoreBonus();

        _countSequence.ChainDelay(1f);
        _countSequence.ChainCallback(() =>
        {
            GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + _roundScore);
            SaveManager.CurrentSave.TotalPoints += _roundScore;
            QuestDirector.Instance.CheckQuestCompletionByType<QD_TotalPoints>();
            GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore);

            if (GameManager.Instance.CurrentScore >= GameManager.Instance.GoalScore)
            {
                //int confettiNumber = 0;
                //if (_roundScore > 0) confettiNumber++;
                //if (_roundScore > GameManager.Instance.GoalScore / 2) confettiNumber++;
                //if (_roundScore > GameManager.Instance.GoalScore) confettiNumber++;
                _confettiLeft.Emit(150);
                _confettiRight.Emit(150);
                AudioManager.PlaySound(AudioManager.Instance.Confetti);
                AudioManager.PlaySound(AudioManager.Instance.Win);
            }

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

    public void CountItems()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            Item_ScoreCalculation item = chosenItemSlotList[i];
            if (item.ItemInstance.Data.Price > 0) // if not garbage
            {
                _countSequence.ChainDelay(1f);
                _countSequence.ChainCallback(() =>
                {
                    CameraManager.Instance.SimpleShake();
                    item.CurrentScore = item.ItemInstance.CalculateValue();
                    _baseScore += item.CurrentScore;
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + item.CurrentScore, item.InfoSpawnPoint.position, GameDirector.Instance.AddColor, UI_TextPopper.AnimSpeed.Quick);
                    item.CountBaseScore();
                    AudioManager.PlaySound(AudioManager.Instance.CountItem);
                });
                _countSequence.ChainDelay(.5f);
                _countSequence.ChainCallback(() =>
                {
                    GameManager.Instance.UIManager.BagMenu.AddScoreText.SetTextValueNumber(Mathf.RoundToInt(_baseScore - item.CurrentScore), (int)_baseScore, .6f);
                });
            }
            else
            {
                _countSequence.ChainDelay(.8f);
                _countSequence.ChainCallback(() =>
                {
                    item.CurrentScore = item.ItemInstance.CalculateValue();
                    item.SetPriceText(item.CurrentScore);
                });

            }

            CheckItemBonus(item);
        }
        _countSequence.ChainDelay(1f);
    }

    public CombinationData CheckCombination(List<ItemData> itemDataList)
    {
        List<CombinationData> combinationList = DataLoader.Instance.CombinationDataDictionary.Values.ToList();
        combinationList = combinationList.OrderByDescending(x => x.Priority).ToList();

        for (int i = 0; i < combinationList.Count; i++)
        {
            List<Item_ScoreCalculation> refChosenItemSlotList = new();
            if (combinationList[i].CheckCombination(itemDataList, ref refChosenItemSlotList))
            {
                return combinationList[i];
            }
        }

        return null;
    }

    public void CheckItemBonus(Item_ScoreCalculation item)
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();
        List<BonusData> bonusList = SaveManager.Instance.CurrentRunBonusList.FindAll(x => x.Effect == BonusEffect.Item); 

        for (int i = 0; i < bonusList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);
            if (bonusList[index].CheckBonus(ref refChosenItemSlotList, CurrentCombinationData) && refChosenItemSlotList.Contains(item))
            {
                BonusBehavior bonus = BonusHandManager.Instance.GetBonus(bonusList[index].Name);

                if (bonusList[index].Points > 0)
                {
                    int bonusValue = Mathf.RoundToInt(bonusList[index].Points);

                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        HighlightBonus(bonusList[index].Name);
                        _baseScore += bonusValue;
                        GameManager.Instance.UIManager.BagMenu.AddScoreText.ScaleAnim(2f, .3f);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue, bonus.InfoSpawnPoint.position, GameDirector.Instance.AddColor, UI_TextPopper.AnimSpeed.Quick);
                        AudioManager.PlaySound(AudioManager.Instance.ScoreAdd);
                        CameraManager.Instance.SimpleShake();
                    });
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        GameManager.Instance.UIManager.BagMenu.AddScoreText.SetTextValueNumber(Mathf.RoundToInt(_baseScore - bonusValue), (int)_baseScore, .6f);
                    });
                }

                if (bonusList[index].Mult > 0)
                {
                    int bonusValue = Mathf.RoundToInt(bonusList[index].Mult);

                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        HighlightBonus(bonusList[index].Name);
                        _multiplierScore += bonusValue;
                        GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.SetTextValueNumber(Mathf.RoundToInt(_multiplierScore - bonusValue), (int)_multiplierScore, .6f);
                        GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.ScaleAnim(2f, .3f);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue, bonus.InfoSpawnPoint.position, GameDirector.Instance.MultiplyColor, UI_TextPopper.AnimSpeed.Quick);
                        AudioManager.PlaySound(AudioManager.Instance.ScoreMultiply);
                        CameraManager.Instance.SimpleShake();
                    });
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.SetTextValueNumber(Mathf.RoundToInt(_multiplierScore - bonusValue), (int)_multiplierScore, .6f);
                    });
                }

                if (bonusList[index].PP > 0)
                {
                    int bonusValue = Mathf.RoundToInt(bonusList[index].PP);

                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        HighlightBonus(bonusList[index].Name);
                        SaveManager.Instance.AddPP(bonusValue, bonus.transform.position);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue + "<sprite name=PP>", bonus.InfoSpawnPoint.position, GameDirector.Instance.MultiplyColor, UI_TextPopper.AnimSpeed.Quick);
                        AudioManager.PlaySound(AudioManager.Instance.ScoreMultiply);
                        CameraManager.Instance.SimpleShake();
                    });
                }
            }
        }
    }

    public void CheckFinalBonus()
    {
        List<Item_ScoreCalculation> chosenItemSlotList = GetChosenItemList();
        List<BonusData> bonusList = SaveManager.Instance.CurrentRunBonusList.FindAll(x => x.Effect == BonusEffect.Final);

        for (int i = 0; i < bonusList.Count; i++)
        {
            int index = i;
            List<Item_ScoreCalculation> refChosenItemSlotList = new(chosenItemSlotList);

            if (bonusList[index].CheckBonus(ref refChosenItemSlotList, CurrentCombinationData))
            {
                BonusBehavior bonus = BonusHandManager.Instance.GetBonus(bonusList[index].Name);

                if (bonusList[index].Points > 0)
                {
                    int bonusValue = Mathf.RoundToInt(bonusList[index].Points);

                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        HighlightBonus(bonusList[index].Name);
                        _baseScore += bonusValue;
                        GameManager.Instance.UIManager.BagMenu.AddScoreText.ScaleAnim(2f, .3f);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue, bonus.InfoSpawnPoint.position, GameDirector.Instance.AddColor, UI_TextPopper.AnimSpeed.Quick);
                        AudioManager.PlaySound(AudioManager.Instance.ScoreAdd);
                        CameraManager.Instance.SimpleShake();
                    });
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        GameManager.Instance.UIManager.BagMenu.AddScoreText.SetTextValueNumber(Mathf.RoundToInt(_baseScore - bonusValue), (int)_baseScore, .6f);
                    });
                }

                if (bonusList[index].Mult > 0)
                {
                    int bonusValue = Mathf.RoundToInt(bonusList[index].Mult);

                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        HighlightBonus(bonusList[index].Name);
                        _multiplierScore += bonusValue;
                        GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.ScaleAnim(2f, .3f);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue, bonus.InfoSpawnPoint.position, GameDirector.Instance.MultiplyColor, UI_TextPopper.AnimSpeed.Quick);
                        AudioManager.PlaySound(AudioManager.Instance.ScoreMultiply);
                        CameraManager.Instance.SimpleShake();
                    });
                    _countSequence.ChainDelay(.5f);
                    _countSequence.ChainCallback(() =>
                    {
                        GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.SetTextValueNumber(Mathf.RoundToInt(_multiplierScore - bonusValue), (int)_multiplierScore, .6f);
                    });
                }

                _countSequence.ChainDelay(.5f);
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
            if (GameManager.Instance.ItemManager.ItemList.Count <= 0)
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

    [Button]
    public void DebugShowOrHide()
    {
        if (_sortingGroup.sortingLayerName != "ScoreCalculation")
        {
            _sortingGroup.sortingLayerName = "ScoreCalculation";
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            FindFirstObjectByType<UI_ScoreCalculation>(FindObjectsInactive.Include).gameObject.SetActive(true);
            FindFirstObjectByType<UIManager>().HUD_Game.gameObject.SetActive(false);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 10.8f, transform.localPosition.z);
            _sortingGroup.sortingLayerName = "Background";
            FindFirstObjectByType<UI_ScoreCalculation>(FindObjectsInactive.Include).gameObject.SetActive(false);
            FindFirstObjectByType<UIManager>().HUD_Game.gameObject.SetActive(true);
        }
    }
#endif
}
