using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BonusEntry : MonoBehaviour
{
    public BonusData Data;
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private TextMeshProUGUI _bonusDescription;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Image _bonusIcon;

    public void SetupBonus(BonusData data)
    {
        Data = data;
        _bonusName.text = Data.Name;
        _bonusDescription.text = Data.Description;
        _bonusIcon.sprite = Data.Icon;
    }

    public void ChooseBonus()
    {
        GameManager.Instance.AddBonus(Data);
        Data = null;
        GameManager.Instance.UIManager.BonusMenu.CloseMenu();
        GameManager.Instance.NextDay();
        GameManager.Instance.SetGameState(GameManager.GameState.ScavengingIntro);
    }
}
