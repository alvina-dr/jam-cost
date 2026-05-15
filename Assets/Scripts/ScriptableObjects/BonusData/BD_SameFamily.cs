using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_SameFamily", menuName = "Scriptable Objects/Bonus/BD_SameFamily")]
public class BD_SameFamily : BonusData
{
    public ItemFamily Family;

    public override void GetBonus()
    {
        base.GetBonus();
        List<ItemData> itemDataList = ItemDirector.Instance.ItemDataDictionary.Values.ToList();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            itemDataList[i].Save.Family = Family;
        }
    }
}
