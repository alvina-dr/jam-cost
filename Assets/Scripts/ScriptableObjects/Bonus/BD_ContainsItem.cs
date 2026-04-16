using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_ContainsTrash", menuName = "Scriptable Objects/Bonus/BD_ContainsTrash")]
public class BD_ContainsItem : BonusData
{
    [SerializeField] private List<ItemData> _containsItemDataList;

    public override void GetBonus()
    {
        base.GetBonus();
    }

    public override bool CheckBonus(ref List<UI_BagSlot> itemDataListRef)
    {
        List<UI_BagSlot> itemDataList = new List<UI_BagSlot>(itemDataListRef);
        List<ItemData> requiredItemList = new(_containsItemDataList);
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (requiredItemList.Find(x => x.Name == itemDataList[i].CurrentBagItem.Data.Name))
            {
                return true;
            }
        }
        return false;
    }
}
