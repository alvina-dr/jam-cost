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
    [SerializeField] private UI_TextValue _totalText;
    [SerializeField] private UI_TextValue _scoreGoalText;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;

    private void Awake()
    {
        UpdateScoreTexts(false);
    }

    public void UpdateScoreTexts(bool animate = true)
    {
        _totalText.SetTextValue(GameManager.Instance.CurrentScore + "$", animate);
        _scoreGoalText.SetTextValue(GameManager.Instance.RoundData.RoundDataList[GameManager.Instance.CurrentDay].ScoreGoal + "$", animate);
    }

    public void ResetTicket()
    {
        for (int i = _ticketEntryList.Count - 1; i >= 0; i--)
        {
            Destroy(_ticketEntryList[i].gameObject);
        }

        _ticketEntryList.Clear();
    }

    public void RemoveItemFromTicket(UI_TicketEntry ticketEntry)
    {
        _ticketEntryList.Remove(ticketEntry);
        Destroy(ticketEntry.gameObject);
    }

    public void AddItemToList(ItemData data)
    {
        UI_TicketEntry ticketEntry = Instantiate(_ticketEntryPrefab, _layout);
        ticketEntry.Setup(data);
        _ticketEntryList.Add(ticketEntry);
    }

    public void CountScore()
    {
        List<ItemData> itemDataList = new();
        Sequence countAnimation = DOTween.Sequence();

        // calculate how many object there is of each family
        List<int> familyCountList = new();
        for (int i = 0; i < Enum.GetNames(typeof(ItemData.ItemFamily)).Length; i++)
        {
            Debug.Log((ItemData.ItemFamily)i);
            familyCountList.Add(_ticketEntryList.FindAll(x => x.Data.Family == (ItemData.ItemFamily)i).Count);
        }

        for (int i = 0; i < _ticketEntryList.Count; i++)
        {
            int index = i;
            int score = _ticketEntryList[index].Data.Price;
            countAnimation.AppendCallback(() => _ticketEntryList[index].BumpPrice());
            countAnimation.Join(_ticketEntryList[index].transform.DOShakeRotation(.3f, .3f));

            // Basic score
            countAnimation.AppendCallback(() =>
            {
                if (familyCountList[(int)_ticketEntryList[index].Data.Family] > 1)
                    GameManager.Instance.UIManager.TextPopperManager.PopText("+" + _ticketEntryList[index].Data.Price, _ticketEntryList[index].ScoreSpawnPoint.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                else
                    GameManager.Instance.UIManager.TextPopperManager.PopText("+" + _ticketEntryList[index].Data.Price, _ticketEntryList[index].ScoreSpawnPoint.position, _addColor);
            });

            // If several in family
            if (familyCountList[(int)_ticketEntryList[index].Data.Family] > 1)
            {
                countAnimation.AppendInterval(.45f);
                countAnimation.AppendCallback(() =>
                {
                    score *= familyCountList[(int)_ticketEntryList[index].Data.Family];
                    GameManager.Instance.UIManager.TextPopperManager.PopText("x" + familyCountList[(int)_ticketEntryList[index].Data.Family], _ticketEntryList[index].ScoreSpawnPoint.position, _multiplyColor);
                });
            }
            countAnimation.AppendCallback(() => GameManager.Instance.CurrentScore += score);
            countAnimation.AppendCallback(() => _totalText.SetTextValue(GameManager.Instance.CurrentScore.ToString() + "$"));
            countAnimation.Append(_totalText.transform.DOShakePosition(.2f, 10));
            countAnimation.AppendInterval(.8f);
            itemDataList.Add(_ticketEntryList[index].Data);
        }
        countAnimation.AppendInterval(2f);
        countAnimation.AppendCallback(() => ResetTicket());
        countAnimation.AppendInterval(1f);
        countAnimation.AppendCallback(() => GameManager.Instance.CheckScoreHighEnough());
        //countAnimation.AppendCallback(() => GameManager.Instance.SetGameState(GameManager.GameState.ChoosingBonus));
        countAnimation.Play();
    }
}
