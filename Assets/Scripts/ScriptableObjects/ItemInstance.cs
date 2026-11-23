using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData Data;
    public ItemTagData TagData;

    public int CalculatePoints()
    {
        if (TagData) return TagData.CalculatePoints(Data);
        return Data.Price;
    }

    public float CalculateMult()
    {
        if (TagData) return TagData.CalculateMult(Data);
        return 0;
    }
}
