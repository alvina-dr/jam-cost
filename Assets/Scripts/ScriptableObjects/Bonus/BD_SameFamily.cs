using UnityEngine;

[CreateAssetMenu(fileName = "BD_SameFamily", menuName = "Scriptable Objects/Bonus/BD_SameFamily")]
public class BD_SameFamily : BonusData
{
    public ItemData.ItemFamily Family;

    public override void GetBonus()
    {
        base.GetBonus();
        for (int i = 0; i < DataLoader.Instance.ItemDataList.Count; i++)
        {
            DataLoader.Instance.ItemDataList[i].Family = Family;
        }
    }
}
