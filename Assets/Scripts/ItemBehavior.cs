using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private ItemData _data;

    private bool _isDragging = false;
    private Vector3 _dragOffset;

    private void OnMouseEnter()
    {
        GameManager.Instance.UIManager.HoverPrice.ShowPrice(_data.Price, transform.position);
    }

    private void OnMouseExit()
    {
        GameManager.Instance.UIManager.HoverPrice.HidePrice();
    }

    private void OnMouseDown()
    {
        _isDragging = true;
        _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        _dragOffset = Vector3.zero;
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
