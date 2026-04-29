using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ItemBehavior : MonoBehaviour
{
    public ItemInstance Item;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    [Header("Shadow")]
    [SerializeField] protected Vector3 _offset;
    [SerializeField] protected Vector3 _startDragPosition;
    [SerializeField] protected Vector3 _flyingOffset;
    [SerializeField] protected SpriteRenderer _shadowSpriteRenderer;

    [Header("Materials")]
    [SerializeField] private Material _family1Material;
    [SerializeField] private Material _family2Material;
    [SerializeField] private Material _family3Material;
    [SerializeField] private Material _family4Material;
    [SerializeField] private Material _familyGarbageMaterial;
    [SerializeField] private Material _clickableMaterial;

    public void Setup(ItemData itemData)
    {
        Item = new();
        Item.Data = itemData;

        _shadowSpriteRenderer.transform.localPosition = _offset;
        _shadowSpriteRenderer.transform.localRotation = Quaternion.identity;

        _shadowSpriteRenderer.sprite = _spriteRenderer.sprite;

        _shadowSpriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
        _shadowSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;

        switch (Item.Data.Family)
        {
            case ItemData.ItemFamily.Plastic:
                _spriteRenderer.material = _family1Material;
                break;
            case ItemData.ItemFamily.Paper:
                _spriteRenderer.material = _family2Material;
                break;
            case ItemData.ItemFamily.Glass:
                _spriteRenderer.material = _family3Material;
                break;
            case ItemData.ItemFamily.Electronics:
                _spriteRenderer.material = _family4Material;
                break;
            case ItemData.ItemFamily.Garbage:
                _spriteRenderer.material = _familyGarbageMaterial;
                break;
            case ItemData.ItemFamily.Clickable:
                _spriteRenderer.material = _clickableMaterial;
                break;
        }

    }

    public void SetTag(ItemTagData tagData)
    {
        if (tagData == null) return;

        Item.TagData = Instantiate(tagData);
        Item.TagData.SetupTag(_spriteRenderer.material);
    }

    public void RemoveTag()
    {
        Item.TagData = null;
    }

    public void SetSortingOrder(int sortingOrder)
    {
        _spriteRenderer.sortingOrder = sortingOrder;
        _shadowSpriteRenderer.sortingOrder = sortingOrder - 1;
        transform.position = new Vector3(transform.position.x, transform.position.y, sortingOrder * -0.001f);
    }

    public void DestroyItem()
    {
        transform.DOKill();
        GameManager.Instance.ItemManager.ItemList.Remove(this);
        Destroy(gameObject);
    }

    public bool CanClickItem()
    {
        if (DialogueManager.Instance.DialogueRunner.IsDialogueRunning) return false;
        if (GameManager.Instance.CurrentGameState == GameManager.Instance.ScavengingState)
        {
            if (GameManager.Instance.ScavengingState.CurrentSubState == GS_Scavenging.Scavenging_SubState.Scavenging) return true;
            else return false;
        }
        if (GameManager.Instance.CurrentGameState == GameManager.Instance.PreparationState)
        {
            if (GameManager.Instance.PreparationState.CurrentSubState == GS_Preparation.Preparation_SubState.Preparation) return true;
            else return false;
        }
        if (GameManager.Instance.CurrentGameState == GameManager.Instance.RewardState) return true;
        return false;
    }

    protected virtual void OnMouseEnter()
    {
        if (!CanClickItem()) return;
        if (GameManager.Instance.SelectedItem != null) return;

        Color color = _spriteRenderer.material.GetColor("_OutlineColor");
        color = new Color(color.r, color.g, color.b, 1);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }

    protected virtual void OnMouseExit()
    {
        if (!CanClickItem()) return;
        if (GameManager.Instance.SelectedItem != null) return;

        Color color = _spriteRenderer.material.GetColor("_OutlineColor");
        color = new Color(color.r, color.g, color.b, 0);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
