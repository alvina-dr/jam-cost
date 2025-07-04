using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TicketMenu : MonoBehaviour
{
    [SerializeField] private Transform _layout;
    [SerializeField] private UI_TicketEntry _ticketEntryPrefab;
    [SerializeField] private List<UI_TicketEntry> _ticketEntryList = new();

    public void ResetTicket()
    {
        for (int i = _ticketEntryList.Count - 1; i >= 0; i--)
        {
            Destroy(_ticketEntryList[i].gameObject);
        }

        _ticketEntryList.Clear(); 
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
}
