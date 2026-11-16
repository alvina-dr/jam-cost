using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehavior : MonoBehaviour
{
    public BonusInstance BonusInstance;
    public BonusBehavior_AddSticker BehaviorAddSticker;
    public OverCheck OverCheck;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    public Transform InfoSpawnPoint;
    [SerializeField] private AppliedStickerBehavior _stickerPrefab;
    [SerializeField] private List<AppliedStickerBehavior> _appliedStickerList = new();
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
            AppliedStickerBehavior appliedSticker = Instantiate(_stickerPrefab, _spriteRenderer.transform);
            appliedSticker.Setup(BonusInstance.StickerList[i]);
            _appliedStickerList.Add(appliedSticker);
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

        AudioManager.PlaySound(AudioManager.Instance.BonusHighlight);
    }

    public void HighlightSticker(string stickerName)
    {
        AppliedStickerBehavior appliedSticker = _appliedStickerList.Find(x => x.StickerInstance.Data.Name == stickerName);
        if (appliedSticker) appliedSticker.Highlight();

        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(transform, 1.2f, .1f));
        sequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, .3f, .1f));
        sequence.Group(Tween.ScaleX(transform, -1.2f, .1f, Ease.Linear));
        sequence.Chain(Tween.ScaleX(transform, 1.2f, .1f, Ease.Linear));
        sequence.Chain(Tween.ScaleX(transform, -1.2f, .1f, Ease.Linear));
        sequence.Chain(Tween.ScaleX(transform, 1.2f, .1f, Ease.Linear));
        sequence.Chain(Tween.Scale(transform, 1f, .1f));
        sequence.Group(Tween.LocalPositionY(_spriteRenderer.transform, 0, .1f));
    }

    public void OnMouseEnter()
    {
        if (!enabled) return;
        if (_isDragged) return;

        TooltipManager.Instance.ShowTooltip(BonusInstance, transform.position, Vector3.up * 200);
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
        AppliedStickerBehavior appliedSticker = Instantiate(_stickerPrefab, _spriteRenderer.transform);
        appliedSticker.transform.position = stickerBehavior.transform.position;
        appliedSticker.Setup(new StickerInstance(stickerBehavior.StickerData, appliedSticker.transform.localPosition));
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
            BonusInstance.AddSticker(stickerBehavior, appliedSticker);
        });
        sequence.ChainDelay(.8f);
        sequence.ChainCallback(() => SaveManager.Instance.NextNode());
    }
}
