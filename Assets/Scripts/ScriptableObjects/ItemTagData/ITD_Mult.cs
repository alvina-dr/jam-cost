using UnityEngine;

[CreateAssetMenu(fileName = "ITD_Mult", menuName = "Scriptable Objects/ItemTagData/ITD_Mult")]
public class ITD_Mult : ItemTagData
{
    public int Mult;

    public override float CalculateMult(ItemData data)
    {
        return Mult;
    }
}
