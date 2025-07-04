using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TicketMenu : MonoBehaviour
{
    public UI_OverCheck OverCheck;
    [SerializeField] private Transform _layout;
    [SerializeField] private UI_TicketEntry _ticketEntryPrefab;
    [SerializeField] private List<UI_TicketEntry> _ticketEntryList = new();
    [SerializeField] private UI_TextValue _totalText;

    private void Awake()
    {
        _totalText.SetTextValue("0$", false);
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

    public bool IsOverTicketMenu()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void CountScore()
    {
        List<ItemData> itemDataList = new();
        Sequence countAnimation = DOTween.Sequence();
        for (int i = 0; i < _ticketEntryList.Count; i++)
        {
            int index = i;
            countAnimation.AppendCallback(() => _ticketEntryList[index].BumpPrice());
            countAnimation.AppendCallback(() => GameManager.Instance.CurrentScore += _ticketEntryList[index].Data.Price);
            countAnimation.AppendCallback(() => _totalText.SetTextValue(GameManager.Instance.CurrentScore.ToString() + "$"));
            countAnimation.Append(_totalText.transform.DOShakePosition(.2f, 10));
            countAnimation.AppendInterval(.8f);
            itemDataList.Add(_ticketEntryList[index].Data);
        }
        countAnimation.AppendInterval(2f);
        countAnimation.AppendCallback(() => ResetTicket());
        countAnimation.AppendInterval(1f);
        countAnimation.AppendCallback(() => GameManager.Instance.SetGameState(GameManager.GameState.ChoosingBonus));
        countAnimation.Play();
    }
}
