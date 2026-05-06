using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_TrashEffect", menuName = "Scriptable Objects/Bonus/BD_TrashEffect")]
public class BD_TrashEffect : BonusData
{
    public float ChancePercentage;

    public bool CheckBonus()
    {
        int random = Random.Range(0, 100);
        return random < ChancePercentage;
    }
}
