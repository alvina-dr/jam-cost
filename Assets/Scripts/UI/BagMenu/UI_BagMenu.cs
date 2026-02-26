using DG.Tweening;
using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_BagMenu : MonoBehaviour
{
    [SerializeField] private UI_Menu _menu;
    [SerializeField] private List<UI_BagSlot> _bagSlotList;
    [SerializeField] private List<UI_BagSlot> _choiceSlotList;
    public Transform DraggedItem;
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

    public void OpenMenu()
    {
        _confirm.gameObject.SetActive(true);
        _continue.gameObject.SetActive(false);

        _menu.OpenMenu();

        _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal, false);
        _currentScoreText.SetTextValue($"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}", false);
        _roundScoreText.SetTextValue($"{0}", false);
        _roundScoreParent.gameObject.SetActive(false);

        List<ItemData> bagItemList = GameManager.Instance.ScavengingState.GetItemDataList();
        GameManager.Instance.ScavengingState.CleanItemDataList();

        for (int i = 0; i < _choiceSlotList.Count; i++)
        {
            _choiceSlotList[i].ClearSlot();

            if (i < bagItemList.Count)
            {
                _choiceSlotList[i].CreateItem(bagItemList[i]);
            }
        }

        //for (int i = 0; i < _choiceSlotList.Count; i++)
        //{
        //    _choiceSlotList[i].ClearSlot();
        //}
    }

    public void CloseMenu()
    {
        _menu.CloseMenu();
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
        _roundScoreText.SetTextValue($"{roundScore}");
        _roundScoreParent.gameObject.SetActive(true);

        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();
        DG.Tweening.Sequence countAnimation = DOTween.Sequence();

        for (int i = 0; i < _bagSlotList.Count; i++)
        {
            int index = i;
            if (_bagSlotList[i].CurrentBagItem != null)
            {
                countAnimation.AppendCallback(() =>
                {
                    _bagSlotList[index].ClearSlot();
                });
                countAnimation.AppendInterval(.1f);
            }
        }

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
            float delay = 0;
            int countTimeMax = 1;

            // BONUS : count items twice on the last round
            BonusData doubleTrouble = SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Find(x => x is BD_LastTurn_DoubleItem);
            if (GameManager.Instance.CurrentRound >= SaveManager.Instance.GetScavengeNode().RoundNumber && doubleTrouble != null)
            {
                countTimeMax = 2;
            }

            for (int countTime = 0; countTime < countTimeMax; countTime++)
            {
                int score = chosenItemSlotList[index].CurrentBagItem.Data.Price;

                // BONUS : DOUBLE TROUBLE
                if (countTime == 1)
                {
                    countAnimation.AppendInterval(.3f);
                    countAnimation.AppendCallback(() =>
                    {
                        GameManager.Instance.UIManager.TextPopperManager_Info.PopText(doubleTrouble.Name, chosenItemSlotList[index].transform.position + Vector3.up, Color.black, UI_TextPopper.AnimSpeed.Quick);
                    });
                    countAnimation.AppendInterval(.1f);
                }

                // Basic score
                if (chosenItemSlotList[index].CurrentBagItem.Data.Price > 0) // if not garbage
                {
                    countAnimation.AppendCallback(() =>
                    {
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + chosenItemSlotList[index].CurrentBagItem.Data.Price, chosenItemSlotList[index].transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                        chosenItemSlotList[index].SetPriceText(score);
                    });
                    countAnimation.AppendInterval(.5f);
                    delay += .45f;
                }


                // COMBINAISON : If several in family
                if (familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family] > 1 && chosenItemSlotList[index].CurrentBagItem.Data.Price > 0)
                {
                    countAnimation.AppendInterval(delay);
                    delay += .3f;
                    countAnimation.AppendCallback(() =>
                    {
                        score += familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family];
                        chosenItemSlotList[index].SetPriceText(score);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family], chosenItemSlotList[index].transform.position, _addColor);
                        ShakeList(chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data.Family == chosenItemSlotList[index].CurrentBagItem.Data.Family));
                    });
                }

                // COMBINAISON : if several time the same
                if (cloneNumber > 1 && chosenItemSlotList[index].CurrentBagItem.Data.Price > 0)
                {
                    countAnimation.AppendInterval(delay);
                    delay += .3f;
                    countAnimation.AppendCallback((TweenCallback)(() =>
                    {
                        score *= cloneNumber;
                        chosenItemSlotList[index].SetPriceText(score);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + cloneNumber, chosenItemSlotList[index].transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                        ShakeList(chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data.Name == chosenItemSlotList[index].CurrentBagItem.Data.Name));
                    }));
                }

                // BONUS : family type
                List<BonusData> bonusDataFamilyList = SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.FindAll(x => x is BD_FamilyMultiplier familyMultiplier && familyMultiplier.FamilyBonus == chosenItemSlotList[index].CurrentBagItem.Data.Family);
                for (int j = 0; j < bonusDataFamilyList.Count; j++)
                {
                    BD_FamilyMultiplier familyMultiplier = (BD_FamilyMultiplier)bonusDataFamilyList[j];
                    countAnimation.AppendInterval(delay);
                    delay += .3f;
                    countAnimation.AppendCallback((TweenCallback)(() =>
                    {
                        score = Mathf.RoundToInt(score * familyMultiplier.BonusMultiplier);
                        chosenItemSlotList[index].SetPriceText(score);
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + familyMultiplier.BonusMultiplier, chosenItemSlotList[index].transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                        GameManager.Instance.UIManager.TextPopperManager_Info.PopText(familyMultiplier.Name, chosenItemSlotList[index].transform.position + Vector3.up, Color.black, UI_TextPopper.AnimSpeed.Quick);
                    }));
                }

                countAnimation.AppendInterval(.5f);
                countAnimation.AppendCallback(() =>
                {
                    roundScore += score;
                    _roundScoreText.SetTextValueNumber(roundScore - score, roundScore);

                    //GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + score);
                    //_scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);
                    //_currentScoreText.SetTextValue(GameManager.Instance.CurrentScore.ToString());
                    //chosenItemSlotList[index].HidePrice();
                });
            }
            countAnimation.AppendInterval(.8f);
        }
        countAnimation.AppendInterval(1f);
        countAnimation.AppendCallback(() =>
        {
            GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + roundScore);
            _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);
            _currentScoreText.SetTextValue($"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}");
            _roundScoreParent.gameObject.SetActive(false);
            //chosenItemSlotList[index].HidePrice();
        });
        countAnimation.AppendInterval(1f);

        // End of round
        if (GameManager.Instance.CurrentRound >= SaveManager.Instance.GetScavengeNode().RoundNumber)
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

#if UNITY_EDITOR
    [Button]
    private void UpdateBagSlotList()
    {
        _bagSlotList.Clear();
        foreach (UI_BagSlot child in transform.GetComponentsInChildren<UI_BagSlot>(includeInactive: true))
        {
            _bagSlotList.Add(child);
        }
    }
#endif
}
