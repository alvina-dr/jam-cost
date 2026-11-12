using UnityEngine;

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
    None = 0,
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
    Screws = 8,
    Other = 9
}

[System.Serializable]
public class ItemDataSave
{
    public string Name;
    public Rarity Rarity;
    [SerializeField] private ItemFamily _family;
    public ItemIdentity Identity;

    public string CompleteName => $"{Name}_{_family}";

    public ItemFamily Family()
    {
        ItemFamily itemFamily = _family;

        BD_SameFamily bonus = SaveManager.Instance.CheckHasRunBonus<BD_SameFamily>();
        if (bonus) itemFamily = bonus.Family;

        return itemFamily;
    }
}
