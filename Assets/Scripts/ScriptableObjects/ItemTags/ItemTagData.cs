using UnityEngine;

[CreateAssetMenu(fileName = "ItemTagData", menuName = "Scriptable Objects/ItemTagData")]
public class ItemTagData : ScriptableObject
{
    public string Name;

    public virtual void SetupTag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {
        Debug.Log("setup tag none");
        sprite.material.EnableKeyword("NONE");
    }

    public virtual void StartDrag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {

    }

    public virtual void EndDrag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {

    }

    public virtual int CalculateValue(ItemData data)
    {
        return data.Price;
    }
}
