using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class BonusBehavior : MonoBehaviour
{
    public BonusInstance BonusInstance;
    public BonusBehavior_AddSticker BehaviorAddSticker;
    public OverCheck OverCheck;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    public Transform InfoSpawnPoint;
    [SerializeField] private SpriteRenderer _stickerPrefab;
    public bool CanDrag = false;
    
    private Vector3 _startDragPosition;
    private Vector3 _dragOffset;
    private bool _isDragged = false;

    public void Setup(BonusInstance bonusInstance)
    {
        gameObject.SetActive(true);
        BonusInstance = bonusInstance;
        _spriteRenderer.sprite = BonusInstance.Data.Icon;
        _collider.enabled = true;

        for (int i = 0; i < BonusInstance.StickerList.Count; i++)
        {
            SpriteRenderer sprite = Instantiate(_stickerPrefab, _spriteRenderer.transform);
            sprite.sprite = BonusInstance.StickerList[i].Data.Sprite;
            sprite.transform.localPosition = BonusInstance.StickerList[i].Position;
        }
    }

    [Button]
    public void Highlight()
    {
        Color outlineColor = _spriteRenderer.material.GetColor("_OutlineColor");
        _spriteRenderer.material.SetColor("_OutlineColor", Color.white);
        //GameManager.Instance.UIManager.TextPopperManager_Info.PopText(BonusData.Name, transform.position + Vector3.up * 1.5f);

        Sequence highlight = Sequence.Create();
        highlight.Chain(Tween.Scale(transform, 1.4f, .2f));
        highlight.Group(Tween.ShakeLocalRotation(_spriteRenderer.transform, new Vector3(0, 0, 20), .2f));
        highlight.Group(Tween.LocalPositionY(_spriteRenderer.transform, .3f, .2f));
        highlight.ChainDelay(.2f);
        highlight.Chain(Tween.Scale(transform, 1f, .1f));
        highlight.Group(Tween.LocalPositionY(_spriteRenderer.transform, 0, .1f));

        highlight.ChainCallback(() => _spriteRenderer.material.SetColor("_OutlineColor", outlineColor));
    }

    public void OnMouseEnter()
    {
        if (!enabled) return;
        if (_isDragged) return;

        TooltipManager.Instance.ShowTooltip(BonusInstance.Data, transform.position, Vector3.up * 200);
        AudioManager.PlaySound(AudioManager.Instance.BonusHover);
        Tween.Scale(transform, 1.2f, .2f, useUnscaledTime: true);
        Tween.ShakeLocalRotation(_spriteRenderer.transform, new Vector3(0, 0, 10), .2f, useUnscaledTime: true);
        Tween.LocalPositionY(_spriteRenderer.transform, .3f, .2f, useUnscaledTime: true);
    }

    public void OnMouseExit()
    {
        if (!enabled) return;
        if (_isDragged) return;

        TooltipManager.Instance.HideTooltip();
        Tween.Scale(transform, 1, .2f, useUnscaledTime: true);
        Tween.ShakeLocalRotation(_spriteRenderer.transform, new Vector3(0, 0, 10), .2f, useUnscaledTime: true);
        Tween.LocalPositionY(_spriteRenderer.transform, 0, .2f, useUnscaledTime: true);
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        if (!CanDrag) return;

        Tween.LocalPositionY(_spriteRenderer.transform, 0, .2f, useUnscaledTime: true);
        _startDragPosition = transform.position;
        _dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isDragged = true;
        _collider.enabled = false;
        TooltipManager.Instance.HideTooltip();

    }

    public void OnMouseDrag()
    {
        if (!enabled) return;
        if (!CanDrag) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition + _dragOffset;
    }

    private void OnMouseUp()
    {
        if (!enabled) return;
        if (!CanDrag) return;
        _collider.enabled = true;
        _isDragged = false;

        if (StickerManager.Instance != null && StickerManager.Instance.SelectCardZone.IsOver())
        {
            StickerManager.Instance.SelectCard(this);
        }
        else
        {
            GoBackToHand();
        }
    }

    public void GoBackToHand()
    {
        Tween.Position(transform, _startDragPosition, .5f);
    }

    public void UpdateCollider()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }

    public void AddSticker(StickerBehavior stickerBehavior)
    {
        SpriteRenderer sprite = Instantiate(_stickerPrefab, _spriteRenderer.transform);
        sprite.sprite = stickerBehavior.StickerData.Sprite;
        sprite.transform.position = stickerBehavior.transform.position;
        stickerBehavior.gameObject.SetActive(false);
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.PositionX(transform, 0, .5f));
        sequence.Group(Tween.ScaleX(transform, -2, .1f, Ease.Linear));
        sequence.Chain(Tween.ScaleX(transform, 2, .1f, Ease.Linear));
        sequence.Chain(Tween.ScaleX(transform, -2, .1f, Ease.Linear));
        sequence.Chain(Tween.ScaleX(transform, 2, .1f, Ease.Linear));
        sequence.ChainDelay(.5f);
        sequence.Group(Tween.Scale(transform, 2.5f, .3f));
        sequence.Chain(Tween.Position(transform, _startDragPosition, .5f));
        sequence.Group(Tween.Scale(transform, 1.0f, .2f));
        sequence.ChainCallback(() =>
        {
            enabled = true;
            BehaviorAddSticker.enabled = false;
            StickerManager.Instance.StickerPlacement.gameObject.SetActive(false);
            BonusInstance.AddSticker(stickerBehavior, sprite);
        });
        sequence.ChainDelay(.8f);
        sequence.ChainCallback(() => SaveManager.Instance.NextNode());
    }
}
