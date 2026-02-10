using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_PowerSlot : MonoBehaviour
{
    [SerializeField] private GameObject _addObject;
    [SerializeField] private GameObject _priceObject;
    [SerializeField] private GameObject _lockedObject;
    [SerializeField] private TextMeshProUGUI _priceText;

    [Button]
    public void LockPowerSlot()
    {
        _addObject.SetActive(false);
        _priceObject.SetActive(true);
        _lockedObject.SetActive(true);
        _priceText.text = $"{10}<sprite name=MT>";
        //set price
    }

    [Button]
    public void UnlockPowerSlot()
    {
        _addObject.SetActive(true);
        _priceObject.SetActive(false);
        _lockedObject.SetActive(false);
    }

    public void SetPower(PowerData powerData)
    {

    }
}
