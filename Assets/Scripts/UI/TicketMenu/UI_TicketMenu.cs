using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_TicketMenu : MonoBehaviour
{
    public UI_OverCheck OverCheck;
    [SerializeField] private Transform _layout;
    [SerializeField] private UI_TicketEntry _ticketEntryPrefab;
    [SerializeField] private List<UI_TicketEntry> _ticketEntryList = new();
    public int GetTicketEntryCount() => _ticketEntryList.Count; 

    [SerializeField] private UI_TextValue _totalScoreText;
    public UI_TextValue TotalScoreText => _totalScoreText;

    [SerializeField] private UI_TextValue _goalScoreText;
    public UI_TextValue GoalScoreText => _goalScoreText;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;

    [Header("Audio")]
    [SerializeField] private AudioClip _countTicketEntryMoney;

    public void ResetTicket()
    {
        Sequence destroySequence = DOTween.Sequence();
        for (int i = _ticketEntryList.Count - 1; i >= 0; i--)
        {
            int index = i;
            destroySequence.Append(_ticketEntryList[index].HorizontalLayout.transform.DOScale(1.3f, .1f));
            destroySequence.Append(_ticketEntryList[index].HorizontalLayout.transform.DOScale(0f, .1f));
            destroySequence.AppendCallback(() => Destroy(_ticketEntryList[index].gameObject));
        }
        destroySequence.AppendCallback(() => _ticketEntryList.Clear());
    }

    public void RemoveItemFromTicket(UI_TicketEntry ticketEntry)
    {
        _ticketEntryList.Remove(ticketEntry);
        Destroy(ticketEntry.gameObject);
    }

    public bool TryAddItemToList(ItemData data)
    {
        if (_ticketEntryList.Count + 1 > GameManager.Instance.HandSize) return false;
        UI_TicketEntry ticketEntry = Instantiate(_ticketEntryPrefab, _layout);
        ticketEntry.Setup(data);
        _ticketEntryList.Add(ticketEntry);
        return true;
    }

    public void CountScore()
    {
        List<ItemData> itemDataList = new();
        Sequence countAnimation = DOTween.Sequence();

        // calculate how many object there is of each family
        List<int> familyCountList = new();
        for (int i = 0; i < Enum.GetNames(typeof(ItemData.ItemFamily)).Length; i++)
        {
            familyCountList.Add(_ticketEntryList.FindAll(x => x.Data.Family == (ItemData.ItemFamily)i).Count);
        }

        for (int i = 0; i < _ticketEntryList.Count; i++)
        {
            int index = i;
            int score = _ticketEntryList[index].Data.Price;
            countAnimation.AppendCallback(() => AudioManager.Instance.PlaySFXSound(_countTicketEntryMoney));
            countAnimation.AppendCallback(() => _ticketEntryList[index].BumpPrice());
            countAnimation.Join(_ticketEntryList[index].transform.DOShakeRotation(.3f, .3f));

            int cloneNumber = _ticketEntryList.FindAll(x => x.Data == _ticketEntryList[i].Data).Count;
            float delay = 0;

            // Basic score
            if (familyCountList[(int)_ticketEntryList[index].Data.Family] > 1 || cloneNumber > 1)
            {
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + _ticketEntryList[index].Data.Price, _ticketEntryList[index].ScoreSpawnPoint.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                }));
                delay += .45f;
            }
            else
            {
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + _ticketEntryList[index].Data.Price, _ticketEntryList[index].ScoreSpawnPoint.position, _addColor);
                }));
                delay += .5f;
            }

            // If several in family
            if (familyCountList[(int)_ticketEntryList[index].Data.Family] > 1)
            {
                countAnimation.AppendInterval(delay);
                delay += .3f;
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    score += familyCountList[(int)_ticketEntryList[index].Data.Family];
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + familyCountList[(int)_ticketEntryList[index].Data.Family], _ticketEntryList[index].ScoreSpawnPoint.position, _addColor);
                }));
            }

            // if several time the same
            if (cloneNumber > 1)
            {
                countAnimation.AppendInterval(delay);
                delay += .3f;
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    score *= cloneNumber;
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + cloneNumber, _ticketEntryList[index].ScoreSpawnPoint.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                }));
            }

            // Bonus family type
            List<BonusData> bonusDataFamilyList = GameManager.Instance.BonusList.FindAll(x => x is BD_FamilyMultiplier familyMultiplier && familyMultiplier.FamilyBonus == _ticketEntryList[index].Data.Family);
            for (int j = 0; j < bonusDataFamilyList.Count; j++)
            {
                BD_FamilyMultiplier familyMultiplier = (BD_FamilyMultiplier) bonusDataFamilyList[j];
                countAnimation.AppendInterval(delay);
                delay += .3f;
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    score = Mathf.RoundToInt(score * familyMultiplier.BonusMultiplier);
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + familyMultiplier.BonusMultiplier, _ticketEntryList[index].ScoreSpawnPoint.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText(familyMultiplier.Name, GameManager.Instance.UIManager.BonusList.GetBonusTextSpawnPoint(familyMultiplier).position, Color.black, UI_TextPopper.AnimSpeed.Quick);
                }));
            }


            countAnimation.AppendCallback(() => GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + score));
            countAnimation.Append(_totalScoreText.transform.DOShakePosition(.2f, 10));
            countAnimation.AppendInterval(.8f);
            itemDataList.Add(_ticketEntryList[index].Data);
        }
        countAnimation.AppendInterval(2f);
        countAnimation.AppendCallback(() => ResetTicket());
        countAnimation.AppendInterval(1f);

        // End of round
        if (GameManager.Instance.CurrentHand >= GameManager.Instance.HandPerRound)
        {
            countAnimation.AppendCallback(() => 
            {
                GameManager.Instance.CheckScoreHighEnough();
                GameManager.Instance.CurrentHand = 0;
            });
        }
        // Next hand
        else
        {
            countAnimation.AppendCallback(() =>
            {
                GameManager.Instance.SetGameState(GameManager.GameState.ScavengingIntro);
            });
        }
    }
}
