using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemBehavior Prefab;
    public int Price;
    public int BonusCurrency;
    public int Rarity;
    public string Name;
    public Sprite Icon;
    public ItemFamily Family;

    public enum ItemFamily
    {
        Plastic = 0,
        Paper = 1,
        Glass = 2,
        Electronics = 3,
        Garbage = 4
    }
}
