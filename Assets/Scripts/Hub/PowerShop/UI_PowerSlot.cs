using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_PowerSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _addObject;
    [SerializeField] private GameObject _priceObject;
    [SerializeField] private GameObject _lockedObject;
    [SerializeField] private TextMeshProUGUI _priceText;

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
        if (powerData == null) return;


    }
}
