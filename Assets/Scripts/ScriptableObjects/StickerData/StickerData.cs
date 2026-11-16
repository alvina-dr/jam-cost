using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "StickerData", menuName = "Scriptable Objects/StickerData")]
public class StickerData : ScriptableObject
{
    public bool IsAvailableInGame;
    public string Name;
    public Sprite Sprite;
    public Rarity Rarity;
    [TextArea, SerializeField] private string _description;

    [HorizontalGroup("Bonus")]
    [BoxGroup("Bonus/ADD"), HideLabel] public float _points;
    [BoxGroup("Bonus/MULT"), HideLabel] public float _mult;
    [BoxGroup("Bonus/PP"), HideLabel] public float _pp;

    public virtual float Points => _points;
    public virtual float Mult => _mult;
    public virtual float PP => _pp;

    public virtual string GetDescription()
    {
        return _description;
    }

    public virtual bool CheckSticker(BonusBehavior bonusBehavior)
    {
        return true;
    }
}
