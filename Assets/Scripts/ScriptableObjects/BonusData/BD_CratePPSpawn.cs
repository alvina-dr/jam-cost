using UnityEngine;

[CreateAssetMenu(fileName = "BD_CratePPSpawn", menuName = "Scriptable Objects/Bonus/BD_CratePPSpawn")]
public class BD_CratePPSpawn : BonusData
{
    public override void GetBonus()
    {
        base.GetBonus();
        ItemDirector.Instance.ItemDataDictionary["Productivity Point"].Save.Rarity = Rarity.Rare;
    }
}
