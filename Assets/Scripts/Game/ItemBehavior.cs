using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ItemBehavior : MonoBehaviour
{
    public ItemData Data;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    [Header("Shadow")]
    [SerializeField] protected Vector3 _offset;
    [SerializeField] protected Vector3 _startDragPosition;
    [SerializeField] protected Vector3 _flyingOffset;
    [SerializeField] protected SpriteRenderer _shadowSpriteRenderer;

    private void Start()
    {
        _shadowSpriteRenderer.transform.localPosition = _offset;
        _shadowSpriteRenderer.transform.localRotation = Quaternion.identity;

        _shadowSpriteRenderer.sprite = _spriteRenderer.sprite;

        _shadowSpriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
        _shadowSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;
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
}
