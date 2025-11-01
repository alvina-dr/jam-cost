using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BonusEntry : MonoBehaviour
{
    public BonusData Data;
    public Button Button;
    public UI_Button ButtonAnim;
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private Image _bonusNameBackground;
    [SerializeField] private TextMeshProUGUI _bonusDescription;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Image _bonusIcon;
    [SerializeField] private Image _shadow;

    public void SetupBonus(BonusData data)
    {
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Data = data;
        _bonusName.text = Data.Name;
        _bonusDescription.text = Data.Description;
        _bonusIcon.sprite = Data.Icon;
        _bonusNameBackground.color = Data.Color;
        _shadow.color = Data.Color;
    }

    public void ChooseBonus()
    {
        ShopManager.Instance.AddBonus(Data);
        Data = null;
        ShopManager.Instance.UIManager.BonusMenu.CloseMenu();
        SaveManager.Instance.NextDay();
    }
}
