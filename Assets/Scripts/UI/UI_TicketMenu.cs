using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TicketMenu : MonoBehaviour
{
    [SerializeField] private Transform _layout;
    [SerializeField] private UI_TicketEntry _ticketEntryPrefab;

    public void AddItemToList(ItemData data)
    {
        UI_TicketEntry ticketEntry = Instantiate(_ticketEntryPrefab, _layout);
        ticketEntry.Setup(data);
    }

    public bool IsOverTicketMenu()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
