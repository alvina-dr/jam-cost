using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData Data;
    public ItemTagData TagData;

    public int CalculateValue()
    {
        if (TagData) return TagData.CalculateValue(Data);
        return Data.Price;
    }
}
