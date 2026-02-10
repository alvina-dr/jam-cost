using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyPowerSlot : MonoBehaviour
{
    [SerializeField] private PowerData _powerData;

    [Header("Components")]
    [SerializeField] private Image _powerIcon;
    [SerializeField] private Image _powerBackgroundHighlight;
    [SerializeField] private Transform _powerPriceParent;
    [SerializeField] private TextMeshProUGUI _powerPrice;
    [SerializeField] private TextMeshProUGUI _soldOutText;

    public void SetupSlot(PowerData powerData = null)
    {
        if (powerData != null) _powerData = powerData;

        if (_powerData)
        {
            _powerIcon.sprite = _powerData.PowerSprite;
            _powerIcon.gameObject.SetActive(true);
            _soldOutText.gameObject.SetActive(false);
            _powerPrice.text = $"{_powerData.PowerPrice}<sprite name=MT>";
        }
        else
        {
            _powerIcon.gameObject.SetActive(false);
            _soldOutText.gameObject.SetActive(true);
            _powerPriceParent.gameObject.SetActive(false);
            return;
        }

        // if already bought
        if (SaveManager.CurrentSave.UnlockedPowerDataList.Contains(_powerData))
        {
            _powerBackgroundHighlight.gameObject.SetActive(false);
            _powerPriceParent.gameObject.SetActive(false);

            // if equipped
            if (SaveManager.CurrentSave.EquipedPowerDataList.Contains(_powerData))
            {

            }
            else
            {

            }
        }
        else
        {
            _powerBackgroundHighlight.gameObject.SetActive(true);
            _powerPriceParent.gameObject.SetActive(true);
        }
    }

    public void TrySetupTicket()
    {
        HubManager.Instance.PowerShop.SetupTicket(_powerData, this);
    }
}
