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

    public void SetupSlot(PowerData powerData = null)
    {
        if (powerData != null) _powerData = powerData;

        if (powerData == null && _powerData == null) return;

        _powerIcon.sprite = _powerData.PowerSprite;

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
            _powerPrice.text = $"{_powerData.PowerPrice}<sprite name=MT>";
            _powerPriceParent.gameObject.SetActive(true);
        }
    }

    public void TrySetupTicket()
    {
        HubManager.Instance.PowerShop.SetupTicket(_powerData, this);
    }
}
