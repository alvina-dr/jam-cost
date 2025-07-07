using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/Bonus/BonusData")]
public class BonusData : ScriptableObject
{
    public bool IsAvailableInGame;
    public string Name;
    public string Description;
    public Sprite Icon;
    public Color Color;
    public List<BonusData> RequiredBonusList;
}
