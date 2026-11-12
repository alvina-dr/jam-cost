using UnityEngine;

[CreateAssetMenu(fileName = "StickerData", menuName = "Scriptable Objects/StickerData")]
public class StickerData : ScriptableObject
{
    public bool IsAvailableInGame;
    public string Name;
    public Sprite Sprite;
    public Rarity Rarity;
    [SerializeField] private string _description;

    public virtual string GetDescription()
    {
        return _description;
    }
}
