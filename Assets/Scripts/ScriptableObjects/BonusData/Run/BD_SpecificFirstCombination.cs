using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_SpecificFirstCombination", menuName = "Scriptable Objects/Bonus/BD_SpecificFirstCombination")]
public class BD_SpecificFirstCombination : BonusData
{
    [SerializeField] private CombinationData _combination;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        if (!GameManager.Instance.FirstCombinationPlayed) return false;
        return DataLoader.GetInstantiatedCombination(GameManager.Instance.FirstCombinationPlayed) == DataLoader.GetInstantiatedCombination(_combination);
    }

    public override string GetDescription()
    {
        string description = base.GetDescription();
        description = description.Replace("[_combination]", _combination.Data.Name);
        return description;
    }
}
