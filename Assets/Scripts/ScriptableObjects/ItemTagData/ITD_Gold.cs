using UnityEngine;

[CreateAssetMenu(fileName = "ITD_Gold", menuName = "Scriptable Objects/ItemTagData/ITD_Gold")]
public class ITD_Gold : ItemTagData
{
    public int PP;

    public override int CalculatePP(ItemData data)
    {
        return PP;
    }
}
