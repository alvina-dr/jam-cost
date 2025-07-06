using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BonusEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private TextMeshProUGUI _bonusDescription;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private BonusData _bonusData;
    [SerializeField] private Image _bonusIcon;

    public void SetupBonus(BonusData data)
    {
        _bonusData = data;
        _bonusName.text = _bonusData.Name;
        _bonusDescription.text = _bonusData.Description;
        _bonusIcon.sprite = _bonusData.Icon;
    }

    //public void SelectBonus()
    //{
    //    _highlight.SetActive(true);
    //    GameManager.Instance.UIManager.BonusMenu.SelectBonus(this);
    //}

    //public void DeselectBonus()
    //{
    //    _highlight.SetActive(false);
    //}

    public void ChooseBonus()
    {
        GameManager.Instance.AddBonus(_bonusData);
        GameManager.Instance.UIManager.BonusMenu.CloseMenu();
        GameManager.Instance.NextDay();
        GameManager.Instance.SetGameState(GameManager.GameState.ScavengingIntro);
    }
}
