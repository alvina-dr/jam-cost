using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/Bonus/BonusData")]
public class BonusData : ScriptableObject
{
    public bool IsAvailableInGame;
    public BonusDurability Durability;

    public string Name;
    public string Description;
    public Sprite Icon;
    public Color Color;
    public List<BonusData> RequiredBonusList;

    public enum BonusDurability
    {
        Run = 0,
        Permanent = 1
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
