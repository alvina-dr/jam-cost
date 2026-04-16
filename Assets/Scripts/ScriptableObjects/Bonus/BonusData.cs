using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/Bonus/BonusData")]
public class BonusData : ScriptableObject
{
    public bool IsAvailableInGame;
    public BonusDurability Durability;

    [ShowIf("Durability", BonusDurability.Permanent)] public BonusCategory Category;
    public BonusEffect Effect;

    public string Name;
    [TextArea]
    public string Description;
    public int Price;
    public float BonusValue;
    public Sprite Icon;
    public Color Color;
    [ShowIf("Durability", BonusDurability.Permanent)] public List<BonusData> UpgradeBonusList;

    public enum BonusEffect
    {
        ItemAddition = 0,
        ItemMultiplication = 1,
        TotalAddition = 2,
        TotalMultiplication = 3,
        Other = 4 
    }
    public enum BonusDurability
    {
        Run = 0,
        Permanent = 1
    }

    public enum BonusCategory
    {
        Space = 0,
        Luck = 1,
        PP = 2,
        Combination = 3,
        Time = 4,
        Cursor = 5
    }

    public virtual void GetBonus()
    {
        switch (Durability)
        {
            case BonusDurability.Run:
                SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Add(this);
                break;
            case BonusDurability.Permanent:
                SaveManager.CurrentSave.PermanentBonusList.Add(this);
                break;
        }
    }

    public virtual bool CheckBonus(ref List<UI_BagSlot> itemDataListRef, List<CombinationData> combinationDataList = null)
    {
        return false;
    }
}
