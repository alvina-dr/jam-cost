using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PowerSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _addObject;
    [SerializeField] private GameObject _priceObject;
    [SerializeField] private GameObject _lockedObject;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _powerIcon;

    [Header("Values")]
    [SerializeField] private int _powerSlotCost;

    [SerializeField, ReadOnly] private bool _unlocked;
    [SerializeField, ReadOnly] private PowerData _powerData;

    public void Setup(bool unlocked = false, PowerData powerData = null)
    {
        _unlocked = unlocked;
        _powerData = powerData;

        if (_unlocked)
        {
            _addObject.SetActive(true);
            _priceObject.SetActive(false);
            _lockedObject.SetActive(false);
            SetPower(_powerData);
        }
        else
        {
            _powerIcon.gameObject.SetActive(false);
            _addObject.SetActive(false);
            _priceObject.SetActive(true);
            _lockedObject.SetActive(true);
            _priceText.text = $"{_powerSlotCost}<sprite name=MT>";
        }
    }

    public void UnlockPowerSlot()
    {
        if (_unlocked) return;
        if (SaveManager.CurrentSave.MealTickets < _powerSlotCost) return;

        SaveManager.Instance.AddMT(-_powerSlotCost);
        SaveManager.CurrentSave.EquipedPowerMax++;
        Setup(true, _powerData);
    }

    public void SetPower(PowerData powerData)
    {
        if (powerData == null)
        {
            _powerIcon.gameObject.SetActive(false);
            return;
        }

        _powerIcon.gameObject.SetActive(true);
        _powerIcon.sprite = powerData.PowerSprite;
    }
}
