using DG.Tweening;
using MoreMountains.Feedbacks;
using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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

    private int _animationStackedNumber;
    private float _animationTimeScale = 1;

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

        List<ItemData> bagItemList = GameManager.Instance.ScavengingState.GetItemDataList();
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
    }


    private void Update()
    {
        if (!_isOpen)
        {
            Time.timeScale = 1.0f;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Time.timeScale = 2f;
        }
        else
        {
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
        int roundScore = 0;
        float baseDelay = 0.5f;
        int totalAnimation = 0;

        _roundScoreText.SetTextValue($"{roundScore}");
        _roundScoreParent.gameObject.SetActive(true);

        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();
        DG.Tweening.Sequence countAnimation = DOTween.Sequence();

        // calculate how many object there is of each family
        List<int> familyCountList = new();
        for (int i = 0; i < Enum.GetNames(typeof(ItemData.ItemFamily)).Length; i++)
        {
            familyCountList.Add(chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data.Family == (ItemData.ItemFamily)i).Count);
        }

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            int index = i;

            int cloneNumber = chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data == chosenItemSlotList[i].CurrentBagItem.Data).Count;
            int countTimeMax = 1;

            // BONUS : count items twice on the last round
            BD_LastTurn_DoubleItem doubleTrouble = SaveManager.Instance.CheckHasRunBonus<BD_LastTurn_DoubleItem>();
            if (GameManager.Instance.CurrentRound >= GameManager.Instance.GetMaxRoundNumber() && doubleTrouble != null)
            {
                countTimeMax = 2;
            }

            for (int countTime = 0; countTime < countTimeMax; countTime++)
            {
                int score = chosenItemSlotList[index].CurrentBagItem.Data.Price;

                // BONUS : DOUBLE TROUBLE
                if (countTime == 1)
                {
                    totalAnimation++;
                    countAnimation.AppendInterval(baseDelay * _animationTimeScale);
                    countAnimation.AppendCallback(() =>
                    {
                        _shakePlayer.PlayFeedbacks();
                        StackAnim();
                        GameManager.Instance.UIManager.TextPopperManager_Info.PopText(doubleTrouble.Name, chosenItemSlotList[index].transform.position + Vector3.up, Color.black, UI_TextPopper.AnimSpeed.Quick);
                    });
                    countAnimation.AppendInterval(.1f * _animationTimeScale);
                }

                // Basic score
                if (chosenItemSlotList[index].CurrentBagItem.Data.Price > 0) // if not garbage
                {
                    totalAnimation++;
                    countAnimation.AppendInterval(baseDelay * _animationTimeScale);
                    countAnimation.AppendCallback(() =>
                    {
                        _shakePlayer.PlayFeedbacks();
                        StackAnim();
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + chosenItemSlotList[index].CurrentBagItem.Data.Price, chosenItemSlotList[index].transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                        chosenItemSlotList[index].SetPriceText(score);
                    });
                }


                // COMBINAISON : If several in family
                if (familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family] > 1 && chosenItemSlotList[index].CurrentBagItem.Data.Price > 0)
                {
                    totalAnimation++;
                    countAnimation.AppendInterval(baseDelay * _animationTimeScale);
                    countAnimation.AppendCallback(() =>
                    {
                        _shakePlayer.PlayFeedbacks();
                        StackAnim();
                        score += familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family];
                        chosenItemSlotList[index].SetPriceText(score);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family], chosenItemSlotList[index].transform.position, _addColor);
                        ShakeList(chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data.Family == chosenItemSlotList[index].CurrentBagItem.Data.Family));
                    });
                }

                // COMBINAISON : if several time the same
                if (cloneNumber > 1 && chosenItemSlotList[index].CurrentBagItem.Data.Price > 0)
                {
                    totalAnimation++;
                    countAnimation.AppendInterval(baseDelay * _animationTimeScale);
                    countAnimation.AppendCallback(() =>
                    {
                        _shakePlayer.PlayFeedbacks();
                        StackAnim();
                        score *= cloneNumber;
                        chosenItemSlotList[index].SetPriceText(score);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + cloneNumber, chosenItemSlotList[index].transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                        ShakeList(chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data.Name == chosenItemSlotList[index].CurrentBagItem.Data.Name));
                    });
                }

                // BONUS : family type
                List<BD_FamilyMultiplier> bonusDataFamilyList = SaveManager.Instance.CheckHasRunBonusList<BD_FamilyMultiplier>(); 
                for (int j = 0; j < bonusDataFamilyList.Count; j++)
                {
                    BD_FamilyMultiplier familyMultiplier = bonusDataFamilyList[j];

                    if (familyMultiplier.FamilyBonus == chosenItemSlotList[index].CurrentBagItem.Data.Family)
                    {
                        totalAnimation++;

                        countAnimation.AppendInterval(baseDelay * _animationTimeScale);
                        countAnimation.AppendCallback(() =>
                        {
                            _shakePlayer.PlayFeedbacks();
                            StackAnim();
                            score = Mathf.RoundToInt(score * familyMultiplier.BonusMultiplier);
                            chosenItemSlotList[index].SetPriceText(score);
                            GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + familyMultiplier.BonusMultiplier, chosenItemSlotList[index].transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                            GameManager.Instance.UIManager.TextPopperManager_Info.PopText(familyMultiplier.Name, chosenItemSlotList[index].transform.position + Vector3.up, Color.black, UI_TextPopper.AnimSpeed.Quick);
                        });
                    }
                }

                totalAnimation++;
                countAnimation.AppendInterval(.15f * _animationTimeScale);
                countAnimation.AppendCallback(() =>
                {
                    StackAnim();
                    roundScore += score;
                    _roundScoreText.SetTextValueNumber(roundScore - score, roundScore, .4f * _animationTimeScale);
                });
            }

            totalAnimation++;
            countAnimation.AppendInterval(.3f * _animationTimeScale);
            StackAnim();
        }
        countAnimation.AppendInterval(.8f * _animationTimeScale);
        countAnimation.AppendCallback(() =>
        {
            GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + roundScore);
            SaveManager.CurrentSave.TotalPoints += roundScore;
            QuestManager.Instance.CheckQuestCompletionByType<QD_TotalPoints>();
            _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);

            int confettiNumber = 0;
            if (roundScore > 0) confettiNumber++;
            if (roundScore > SaveManager.Instance.GetScavengeNode().ScoreGoal / 2) confettiNumber++;
            if (roundScore > SaveManager.Instance.GetScavengeNode().ScoreGoal) confettiNumber++;
            _confettiLeft.Emit(confettiNumber * 50);
            _confettiRight.Emit(confettiNumber * 50);
            
            _currentScoreText.SetTextValue($"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}");
            _roundScoreParent.gameObject.SetActive(false);
            //chosenItemSlotList[index].HidePrice();
        });
        countAnimation.AppendInterval(1f);

        // End of round
        if (GameManager.Instance.CurrentRound >= GameManager.Instance.GetMaxRoundNumber())
        {
            countAnimation.AppendCallback(() =>
            {
                GameManager.Instance.CheckScoreHighEnough();
                GameManager.Instance.CurrentRound = 0;
            });
        }
        // Next hand
        else
        {
            countAnimation.AppendCallback(() =>
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
            PrimeTween.Sequence anim = PrimeTween.Sequence.Create();
            anim.Chain(PrimeTween.Tween.LocalPositionY(slotList[i].CurrentBagItem.transform, 20, .3f));
            anim.Chain(PrimeTween.Tween.LocalPositionY(slotList[i].CurrentBagItem.transform, 0, .3f));
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
}
