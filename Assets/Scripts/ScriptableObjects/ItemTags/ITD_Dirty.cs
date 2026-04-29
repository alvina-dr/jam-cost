using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "IT_Dirty", menuName = "Scriptable Objects/ItemTags/IT_Dirty")]
public class ITD_Dirty : ItemTagData
{
    public int Dirtiness;

    public override void SetupTag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {
        sprite.material.EnableKeyword("ITEMTAG_DIRTY");
        sprite.material.DisableKeyword("ITEMTAG_NONE");
        sprite.material.SetFloat("_Dirtiness", Dirtiness);
    }

    public override void StartDrag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {

    }

    public override void EndDrag(ItemBehavior itemBehavior, SpriteRenderer sprite)
    {
        if (Dirtiness <= 0) return;

        Dirtiness--;
        sprite.material.SetFloat("_Dirtiness", Dirtiness);

        if (Dirtiness <= 0)
        {
            itemBehavior.RemoveTag();
            sprite.material.EnableKeyword("ITEMTAG_NONE");
            sprite.material.DisableKeyword("ITEMTAG_DIRTY");
        }
    }

    public override int CalculateValue(ItemData data)
    {
        return Mathf.RoundToInt((float) data.Price * ((float) Dirtiness / 2.0f));
    }
}
