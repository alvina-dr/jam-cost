using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BonusIcon : MonoBehaviour
{
    public BonusData Data;
    [SerializeField] private Image _bonusIcon;
    public Transform InfoSpawnPoint;
    [SerializeField] private TextMeshProUGUI _description;

    public void Setup(BonusData data)
    {
        Data = data;
        _bonusIcon.sprite = data.Icon;
        _description.text = data.Description;
    }
}
