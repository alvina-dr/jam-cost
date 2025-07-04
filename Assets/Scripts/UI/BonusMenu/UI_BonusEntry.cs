using TMPro;
using UnityEngine;

public class UI_BonusEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private BonusData _bonusData;

    public void SetupBonus(BonusData data)
    {
        _bonusData = data;
        _bonusName.text = _bonusData.Name;
    }

    public void SelectBonus()
    {
        _highlight.SetActive(true);
        GameManager.Instance.UIManager.BonusMenu.SelectBonus(this);
    }

    public void DeselectBonus()
    {
        _highlight.SetActive(false);
    }

    public void ChooseBonus()
    {
        GameManager.Instance.AddBonus(_bonusData);
        GameManager.Instance.UIManager.BonusMenu.CloseMenu();
        GameManager.Instance.SetGameState(GameManager.GameState.Scavenging);
    }
}
