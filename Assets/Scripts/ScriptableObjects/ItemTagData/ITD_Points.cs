using UnityEngine;

[CreateAssetMenu(fileName = "ITD_Points", menuName = "Scriptable Objects/ItemTagData/ITD_Points")]
public class ITD_Points : ItemTagData
{
    public int Points;

    public override int CalculatePoints(ItemData data)
    {
        return data.Price + Points;
    }
}
