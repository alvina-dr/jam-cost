using TMPro;
using UnityEngine;

public class UI_ConversionMenu : UI_Menu
{
    [SerializeField] private TextMeshProUGUI _conversionText;
    [SerializeField] private Transform _mealTicketSpawnPoint;
    private int _mealTicketNumber;

    public override void OpenMenu()
    {
        UpdateConversionDisplay();
        base.OpenMenu();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }

    public void AddMTConversion()
    {
        if (SaveManager.Instance.CurrentSave.CurrentRun.ProductivityPoints < (_mealTicketNumber + 1) * 5)
        {
            // prevent player from selecting more
            return;
        }
        _mealTicketNumber++;
        UpdateConversionDisplay();
    }

    public void RemoveMTConversion()
    {
        _mealTicketNumber--;
        if (_mealTicketNumber < 0) _mealTicketNumber = 0;
        UpdateConversionDisplay();
    }

    public void UpdateConversionDisplay()
    {
        _conversionText.text = $"{_mealTicketNumber * 5}<sprite name=PP> -> {_mealTicketNumber}<sprite name=MT>";
    }

    public void ConfirmConversion()
    {
        SaveManager.Instance.AddMT(_mealTicketNumber, _mealTicketSpawnPoint.position);
        SaveManager.Instance.AddPP(-_mealTicketNumber*5);
        _mealTicketNumber = 0;
        CloseMenu();
    }

}
