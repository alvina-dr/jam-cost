using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TicketMenu : MonoBehaviour
{
    [SerializeField] private Transform _layout;
    [SerializeField] private UI_TicketEntry _ticketEntryPrefab;
    [SerializeField] private List<UI_TicketEntry> _ticketEntryList = new();
    public int GetTicketEntryCount() => _ticketEntryList.Count; 

    [SerializeField] private UI_TextValue _itemNumberText;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;

    [Header("Audio")]
    [SerializeField] private AudioClip _countTicketEntryMoney;
    [SerializeField] private AudioClip _validateTicketSound;

    [Header("Image")]
    [SerializeField] private Image _ticketImage;
    [SerializeField] private Sprite _normalTicket;
    [SerializeField] private Sprite _greyTicket;

    public void UpdateItemNumberText()
    {
        _itemNumberText.SetTextValue(GetTicketEntryCount() + "/" + GameManager.Instance.GetTicketSize());
    }

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
        destroySequence.AppendCallback(() => UpdateItemNumberText());
    }

    public void RemoveItemFromTicket(UI_TicketEntry ticketEntry)
    {
        _ticketEntryList.Remove(ticketEntry);
        Destroy(ticketEntry.gameObject);
        UpdateItemNumberText();
    }

    public void EnableMenu()
    {
        //OverCheck.enabled = true;
        _ticketImage.sprite = _normalTicket;
    }

    public void DisableMenu()
    {
        //OverCheck.enabled = false;
        _ticketImage.sprite = _greyTicket;
    }
}
