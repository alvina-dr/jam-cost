using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BonusEntry : MonoBehaviour
{
    public BonusData BonusData;
    public Button Button;
    public UI_Button ButtonAnim;
    [SerializeField] private TextMeshProUGUI _bonusPrice;
    [SerializeField] private Image _bonusNameBackground;
    [SerializeField] private TextMeshProUGUI _bonusDescription;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Image _bonusIcon;
    [SerializeField] private Image _shadow;

    public void SetupBonus(BonusData data)
    {
        if (data == null)
        {
            _bonusIcon.gameObject.SetActive(false);
            _bonusPrice.text = $"SOLD OUT";
            return;
        }

        BonusData = data;
        _bonusPrice.text = $"{BonusData.Price} <sprite name=PP>";
        _bonusIcon.gameObject.SetActive(true);
        _bonusIcon.sprite = BonusData.Icon;
    }

    public void TrySetupInfo()
    {
        ShopManager.Instance.BonusMenu.SetupInfo(BonusData, this);
    }
}
