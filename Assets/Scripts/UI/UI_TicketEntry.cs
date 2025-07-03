using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TicketEntry : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;

    public void Setup(ItemData data)
    {
        _itemIcon.sprite = null;
        _itemName.text = data.Name;
    }
}
