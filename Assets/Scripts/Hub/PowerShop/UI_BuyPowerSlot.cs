using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyPowerSlot : MonoBehaviour
{
    [SerializeField] private PowerData _powerData;

    [Header("Components")]
    [SerializeField] private Image _powerIcon;
    [SerializeField] private Transform _powerPriceParent;
    [SerializeField] private TextMeshProUGUI _powerPrice;

    public void SetupSlot(PowerData powerData)
    {
        _powerData = powerData;
        _powerIcon.sprite = powerData.PowerSprite;

        // check si ça a été acheté
        _powerPrice.text = $"{powerData.PowerPrice}<sprite name=MT>";
    }

    public void TrySetupTicket()
    {
        HubManager.Instance.PowerShop.SetupTicket(_powerData);
    }
}
