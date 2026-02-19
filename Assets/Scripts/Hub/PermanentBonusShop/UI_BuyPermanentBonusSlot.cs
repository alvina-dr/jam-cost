using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyPermanentBonusSlot : MonoBehaviour
{
    public BonusData BonusData;

    [SerializeField] private Image _bonusIcon;
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private TextMeshProUGUI _bonusPrice;

    public int CurrentIndex = 0;

    [Header("Bonus Upgrade")]
    [SerializeField] private Transform _bonusUpgradeVisual;
    [SerializeField] private Image _bonusUpgradeLevel;
    [SerializeField] private List<Image> _bonusUpgradeLevelList;

    public void Setup(BonusData bonusData)
    {
        bool everythingBought = true;
        BonusData = bonusData;
        CurrentIndex = 0;

        DestroyAllChildren(_bonusUpgradeVisual.transform);
        _bonusUpgradeLevelList.Clear();

        if (bonusData.UpgradeBonusList.Count > 0)
        {
            Image imageB = Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
            _bonusUpgradeLevelList.Add(imageB);
            if (SaveManager.CurrentSave.PermanentBonusList.Contains(BonusData))
            {
                imageB.color = Color.black;
                CurrentIndex++;
            }
            else
            {
                everythingBought = false;
                imageB.color = Color.white;
            }

            for (int i = 0; i < bonusData.UpgradeBonusList.Count; i++)
            {
                Image image = Instantiate(_bonusUpgradeLevel, _bonusUpgradeVisual);
                _bonusUpgradeLevelList.Add(image);
                if (SaveManager.CurrentSave.PermanentBonusList.Contains(bonusData.UpgradeBonusList[i]))
                {
                    image.color = Color.black;
                    CurrentIndex++;
                }
                else
                {
                    image.color = Color.white;
                    everythingBought = false;
                }
            }
        }
        else
        {
            if (SaveManager.CurrentSave.PermanentBonusList.Contains(BonusData))
            {

            }
            else
            {
                everythingBought = false;
            }
        }

        if (CurrentIndex < _bonusUpgradeLevelList.Count) _bonusUpgradeLevelList[CurrentIndex].color = Color.grey;

        if (everythingBought)
        {
            _bonusPrice.gameObject.SetActive(false);
        }
        else
        {
            _bonusPrice.gameObject.SetActive(true);
        }

        if (CurrentIndex == 0)
        {
            _bonusIcon.sprite = BonusData.Icon;
            _bonusName.text = BonusData.Name;
            _bonusPrice.text = $"{BonusData.Price}<sprite name=MT>";
        }
        else
        {
            int index = CurrentIndex - 1;
            if (CurrentIndex > BonusData.UpgradeBonusList.Count)
            {
                index = BonusData.UpgradeBonusList.Count - 1;
            }
            _bonusIcon.sprite = BonusData.UpgradeBonusList[index].Icon;
            _bonusName.text = BonusData.UpgradeBonusList[index].Name;
            _bonusPrice.text = $"{BonusData.UpgradeBonusList[index].Price}<sprite name=MT>";
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
