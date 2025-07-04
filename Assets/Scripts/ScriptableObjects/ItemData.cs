using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int Price;
    public int Rarity;
    public string Name;
    public Sprite ItemIcon;
}
