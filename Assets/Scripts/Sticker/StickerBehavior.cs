using PrimeTween;
using UnityEngine;

public class StickerBehavior : MonoBehaviour
{
    public StickerData StickerData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;

    private bool _canStick;
    private Vector3 _startDragPosition;
    private Vector3 _dragOffset;
    private bool _isDragged = false;

    public void Setup(StickerData stickerData)
    {
        StickerData = stickerData;
        _spriteRenderer.sprite = stickerData.Sprite;
    }

    public void StickMode()
    {
        _canStick = true;

        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(transform, 1f, .3f));
        sequence.Chain(Tween.Scale(transform, .8f, .2f));
        sequence.Chain(Tween.Position(transform, StickerManager.Instance.StickerPosition.position, .5f));
    }

    private void OnMouseOver()
    {
        TooltipManager.Instance.ShowTooltip(StickerData, transform.position, Vector3.up * 60);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
    }

    private void OnMouseDown()
    {
        if (!_canStick) return;
        _startDragPosition = transform.position;
        _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isDragged = true;
        _collider.enabled = false;
        TooltipManager.Instance.HideTooltip();
    }

    private void OnMouseDrag()
    {
        if (!_canStick) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition + _dragOffset;
    }

    private void OnMouseUp()
    {
        if (!_canStick) return;

        if (StickerManager.Instance.CurrentBonusBehavior.OverCheck.IsOver())
        {
            StickerManager.Instance.CurrentBonusBehavior.AddSticker(this);
        }
        else
        {

        }

        _collider.enabled = true;
        _isDragged = false;
    }
}
