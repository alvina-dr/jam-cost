using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ItemBehavior : MonoBehaviour
{
    public ItemData Data;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _isDragging = false;
    private Vector3 _dragOffset;

    [Header("Shadow")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _startDragPosition;
    [SerializeField] private Vector3 _flyingOffset;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;

    [SerializeField] private SpriteRenderer _cross;
    [SerializeField] private SpriteRenderer _sellIcon;
    
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private AudioClip _trashItemSound;
    [SerializeField] private AudioClip _addToTicketSound;

    private void Start()
    {
        _shadowSpriteRenderer.transform.localPosition = _offset;
        _shadowSpriteRenderer.transform.localRotation = Quaternion.identity;

        _shadowSpriteRenderer.sprite = _spriteRenderer.sprite;

        _shadowSpriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
        _shadowSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;
    }

    private void LateUpdate()
    {

        if (_isDragging)
        {
            _shadowSpriteRenderer.transform.localPosition = Vector3.Lerp(_shadowSpriteRenderer.transform.localPosition,
                Quaternion.Inverse(_spriteRenderer.transform.rotation) * _flyingOffset, Time.deltaTime * 30);
            //_shadowSpriteRenderer.transform.localPosition = Quaternion.Inverse(_spriteRenderer.transform.rotation) * _flyingOffset;
        }
        else
        {
            _shadowSpriteRenderer.transform.localPosition = Vector3.Lerp(_shadowSpriteRenderer.transform.localPosition,
                Quaternion.Inverse(_spriteRenderer.transform.rotation) * _offset, Time.deltaTime * 30);
        }

    }

    private void OnMouseEnter()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        GameManager.Instance.UIManager.HoverPrice.ShowPrice(Data.Price, transform.position);
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        GameManager.Instance.UIManager.HoverPrice.HidePrice();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        StartDrag();
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        EndDrag();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _collider.enabled = true;
                transform.DOScale(1f, .3f).SetEase(Ease.InBack);
                if (GameManager.Instance.SelectedItem == this) GameManager.Instance.SelectedItem = null;
            }
            return;
        }

        if (_isDragging)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseWorldPosition + _dragOffset;

            if (GameManager.Instance.UIManager.TicketMenu.OverCheck.IsOver())
            {
                if (GameManager.Instance.UIManager.TicketMenu.GetTicketEntryCount() + 1 > GameManager.Instance.GetTicketSize()) _sellIcon.color = Color.grey;
                else _sellIcon.color = Color.green;
                _sellIcon.enabled = true;
                _cross.enabled = false;
            }
            else if (!GameManager.Instance.UIManager.DumpsterOverCheck.IsOver())
            {
                _sellIcon.enabled = false;
                _cross.enabled = true;
            }
            else
            {
                _sellIcon.enabled = false;
                _cross.enabled = false;
            }
        }
    }

    public void StartDrag()
    {
        _isDragging = true;
        _startDragPosition = transform.position;
        _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.SelectedItem = this;
        _collider.enabled = false;
        transform.DOScale(1.3f, .4f).SetEase(Ease.OutBack);
        gameObject.layer = LayerMask.NameToLayer("FrontItem");
        _spriteRenderer.sortingLayerName = "Front";
        _shadowSpriteRenderer.sortingLayerName = "Front";
        SetSortingOrder(100);
        AudioManager.Instance.PlaySFXSound(_pickUpSound);
    }

    public void EndDrag()
    {
        _isDragging = false;
        _collider.enabled = true;
        transform.DOScale(1f, .1f).SetEase(Ease.InBack);

        if (GameManager.Instance.SelectedItem == this) GameManager.Instance.SelectedItem = null;

        if (GameManager.Instance.UIManager.TicketMenu.OverCheck.IsOver())
        {
            if (GameManager.Instance.UIManager.TicketMenu.TryAddItemToList(Data))
            {
                transform.DOKill();
                GameManager.Instance.ItemManager.ItemList.Remove(this);
                Destroy(gameObject);
                AudioManager.Instance.PlaySFXSound(_addToTicketSound);
            }
            // if ticket is full, go back to bin
            else
            {
                transform.DOMove(_startDragPosition, .5f).OnComplete(() =>
                {
                    gameObject.layer = LayerMask.NameToLayer("Default");
                    _spriteRenderer.sortingLayerName = "Default";
                    _shadowSpriteRenderer.sortingLayerName = "Default";
                    SetSortingOrder(GameManager.Instance.ItemManager.TopLayer + 2);
                    GameManager.Instance.ItemManager.TopLayer += 2;
                });
                _sellIcon.enabled = false;
                _cross.enabled = false;
            }

        }
        else if (!GameManager.Instance.UIManager.DumpsterOverCheck.IsOver())
        {
            transform.DOKill();
            GameManager.Instance.ItemManager.ItemList.Remove(this);
            Destroy(gameObject);
            AudioManager.Instance.PlaySFXSound(_trashItemSound);
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            _spriteRenderer.sortingLayerName = "Default";
            _shadowSpriteRenderer.sortingLayerName = "Default";
            SetSortingOrder(GameManager.Instance.ItemManager.TopLayer + 2);
            GameManager.Instance.ItemManager.TopLayer += 2;
            AudioManager.Instance.PlaySFXSound(_pickUpSound);
        }
    }

    public void SetSortingOrder(int sortingOrder)
    {
        _spriteRenderer.sortingOrder = sortingOrder;
        _shadowSpriteRenderer.sortingOrder = sortingOrder - 1;
        transform.position = new Vector3(transform.position.x, transform.position.y, sortingOrder * -0.001f);
    }
}
