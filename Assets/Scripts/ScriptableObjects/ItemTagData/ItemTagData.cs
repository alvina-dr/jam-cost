using UnityEngine;

[CreateAssetMenu(fileName = "ItemTagData", menuName = "Scriptable Objects/ItemTagData")]
public class ItemTagData : ScriptableObject
{
    public string Name;
    [TextArea, SerializeField] private string _description;

    public virtual string GetDescription()
    {
        return _description;
    }

    public virtual void SetupTag(Material material)
    {
        material.EnableKeyword("NONE");
    }

    public virtual void StartDrag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {

    }

    public virtual void EndDrag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {

    }

    public virtual int CalculatePoints(ItemData data)
    {
        return data.Price;
    }

    public virtual float CalculateMult(ItemData data)
    {
        return 0;
    }

    public virtual int CalculatePP(ItemData data)
    {
        return 0;
    }
}
