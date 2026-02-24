using UnityEngine;

[CreateAssetMenu(fileName = "BD_PreparationTime", menuName = "Scriptable Objects/Bonus/BD_PreparationTime")]
public class BD_PreparationTime : BonusData
{
    public float PreparationTimeDuration;

    public override void GetBonus()
    {
        base.GetBonus();
    }
}
