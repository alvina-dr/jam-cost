using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_BuyPermanentBonusSlot : MonoBehaviour
{
    public BonusData BonusData;

    [SerializeField] private Image _bonusIcon;
    [SerializeField] private TextMeshProUGUI _bonusName;

    public int CurrentIndex = 0;

    [Header("Bonus Upgrade")]
    [SerializeField] private Transform _bonusUpgradeVisual;
    [SerializeField] private GameObject _bonusUpgradeLevel;

    public void Setup(BonusData bonusData)
    {
        BonusData = bonusData;
        _bonusIcon.sprite = BonusData.Icon;
        _bonusName.text = BonusData.Name;

        DestroyAllChildren(_bonusUpgradeVisual.transform);
        for (int i = 0; i < bonusData.UpgradeBonusList.Count; i++)
        {
            Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
        }

        if (bonusData.UpgradeBonusList.Count > 0) Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
    }

    public void AddBonusUpgrade(BonusData bonusData)
    {
        //BonusData.Add(bonusData);
        Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
    }

    public void TrySetupTicket()
    {
        HubManager.Instance.PermanentBonusShop.SetupTicket(BonusData, this);
    }

    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
