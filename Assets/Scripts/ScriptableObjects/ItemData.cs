using UnityEngine;
using static ItemData;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemBehavior Prefab;
    public int Price;
    public int BonusCurrency;
    public Sprite Icon;
    public ItemDataSave Save;
}

public enum ItemFamily
{
    Plastic = 0,
    Paper = 1,
    Glass = 2,
    Electronics = 3,
    Garbage = 4,
    Clickable = 5
}

[System.Serializable]
public class ItemDataSave
{
    public string Name;
    public Rarity Rarity;
    public ItemFamily Family;
}
