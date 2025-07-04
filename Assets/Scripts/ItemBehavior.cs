using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ItemBehavior : MonoBehaviour
{
    public ItemData Data;
    [SerializeField] private Collider2D _collider;

    private bool _isDragging = false;
    private Vector3 _dragOffset;

    private void OnMouseEnter()
    {
        GameManager.Instance.UIManager.HoverPrice.ShowPrice(Data.Price, transform.position);
    }

    private void OnMouseExit()
    {
        GameManager.Instance.UIManager.HoverPrice.HidePrice();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Scavenging) return;

        _isDragging = true;
        _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.SelectedItem = this;
        _collider.enabled = false;
        transform.DOScale(1.3f, .4f).SetEase(Ease.OutBack);
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        _collider.enabled = true;
        transform.DOScale(1f, .3f).SetEase(Ease.InBack);
        if (GameManager.Instance.SelectedItem == this) GameManager.Instance.SelectedItem = null;

        if (GameManager.Instance.UIManager.TicketMenu.IsOverTicketMenu())
        {
            GameManager.Instance.UIManager.TicketMenu.AddItemToList(Data);
            transform.DOKill();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseWorldPosition + _dragOffset;
        }
    }
}
