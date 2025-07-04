using UnityEngine;
using UnityEngine.UI;

public class UI_BonusIcon : MonoBehaviour
{
    [SerializeField] private BonusData _bonusData;
    [SerializeField] private Image _bonusIcon;

    public void Setup(BonusData data)
    {
        _bonusData = data;
        _bonusIcon.sprite = data.Icon;
    }
}
