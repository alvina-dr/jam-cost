using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ITD_Competitor", menuName = "Scriptable Objects/ItemTagData/ITD_Competitor")]
public class ITD_Competitor : ItemTagData
{
    [SerializeField] private int _mult;

    public override float CalculateMult(ItemData data)
    {
        List<Item_ScoreCalculation> itemScoreCalculationList = ScoreCalculationManager.Instance.GetChosenItemList();
        int numberOfCompetitor = itemScoreCalculationList.FindAll(x => x.ItemInstance.TagData != null && x.ItemInstance.TagData.Name == Name).Count;
        return _mult * numberOfCompetitor;
    }
}
