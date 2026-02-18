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
    [SerializeField] private Image _bonusUpgradeLevel;

    public void Setup(BonusData bonusData)
    {
        BonusData = bonusData;
        CurrentIndex = 0;

        DestroyAllChildren(_bonusUpgradeVisual.transform);

        if (bonusData.UpgradeBonusList.Count > 0)
        {
            Image imageB = Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
            if (SaveManager.CurrentSave.PermanentBonusList.Contains(BonusData))
            {
                imageB.color = Color.black;
                CurrentIndex++;
            }
            else
            {
                imageB.color = Color.white;
            }

            for (int i = 0; i < bonusData.UpgradeBonusList.Count; i++)
            {
                Image image = Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
                if (SaveManager.CurrentSave.PermanentBonusList.Contains(bonusData.UpgradeBonusList[i]))
                {
                    image.color = Color.black;
                    CurrentIndex++;
                }
                else
                {
                    image.color = Color.white;
                }
            }
        }
        else
        {
            if (SaveManager.CurrentSave.PermanentBonusList.Contains(BonusData))
            {
                // make it appear bought already
            }
        }

        if (CurrentIndex == 0) 
        {
            _bonusIcon.sprite = BonusData.Icon;
            _bonusName.text = BonusData.Name;
        }
        else
        {
            if (CurrentIndex > BonusData.UpgradeBonusList.Count) CurrentIndex = BonusData.UpgradeBonusList.Count;
            _bonusIcon.sprite = BonusData.UpgradeBonusList[CurrentIndex - 1].Icon;
            _bonusName.text = BonusData.UpgradeBonusList[CurrentIndex - 1].Name;
        }
    }

    public void TrySetupTicket()
    {
        if (CurrentIndex == 0) HubManager.Instance.PermanentBonusShop.SetupTicket(BonusData, this);
        else HubManager.Instance.PermanentBonusShop.SetupTicket(BonusData.UpgradeBonusList[CurrentIndex - 1], this);
    }

    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
