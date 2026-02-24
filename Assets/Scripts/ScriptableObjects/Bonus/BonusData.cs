using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/Bonus/BonusData")]
public class BonusData : ScriptableObject
{
    public bool IsAvailableInGame;
    public BonusDurability Durability;
    public BonusCategory Category;

    public string Name;
    [TextArea]
    public string Description;
    public int Price;
    public Sprite Icon;
    public Color Color;
    public List<BonusData> UpgradeBonusList;

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
}
