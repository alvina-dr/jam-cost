using NUnit.Framework.Interfaces;
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
    Blue = 1,
    Red = 2,
    Green = 3,
    Garbage = 4,
    Clickable = 5
}

public enum ItemIdentity
{
    Milk = 0,
    Apple = 1,
    Pickles = 2,
    Sock = 3,
    Bra = 4,
    Cap = 5,
    Spray = 6,
    Lighter = 7,
    Screws = 8
}

[System.Serializable]
public class ItemDataSave
{
    public string Name;
    public Rarity Rarity;
    public ItemFamily Family;
    public ItemIdentity Identity;

    public string CompleteName => $"{Name}_{Family}";
}
