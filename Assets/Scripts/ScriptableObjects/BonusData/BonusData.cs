using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/Bonus/BonusData")]
public class BonusData : ScriptableObject
{
    public bool IsAvailableInGame;
    public BonusDurability Durability;
    [ShowIf("Durability", BonusDurability.Run)] public Rarity Rarity;

    [ShowIf("Durability", BonusDurability.Permanent)] public BonusCategory Category;
    public BonusEffect Effect;

    public string Name;
    [TextArea] public string Description;
    public int Price;
    [HorizontalGroup("Bonus")]
    [BoxGroup("Bonus/ADD"), HideLabel] public float _points;
    [BoxGroup("Bonus/MULT"), HideLabel] public float _mult;

    public virtual float Points => _points;
    public virtual float Mult => _mult;

    public Sprite Icon;
    public Color Color;
    [ShowIf("Durability", BonusDurability.Permanent)] public List<BonusData> UpgradeBonusList;

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
                SaveManager.Instance.CurrentRunBonusList.Add(this);
                break;
            case BonusDurability.Permanent:
                SaveManager.Instance.PermanentBonusList.Add(this);
                break;
        }
    }

    public virtual bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        return false;
    }

    public virtual string GetDescription()
    {
        Description = Description.Replace("[_points]", _points.ToString());
        Description = Description.Replace("[_mult]", _mult.ToString());
        Description = Description.Replace("[Mult]", Mult.ToString());
        Description = Description.Replace("[Points]", Points.ToString());
        return Description;
    }
}

public enum BonusEffect
{
    Item = 0,
    Final = 1,
    TotalMultiplication = 2,
    Other = 3
}

public enum BonusDurability
{
    Run = 0,
    Permanent = 1
}
