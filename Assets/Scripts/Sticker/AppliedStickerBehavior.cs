using PrimeTween;
using UnityEngine;

public class AppliedStickerBehavior : MonoBehaviour
{
    public StickerInstance StickerInstance;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _highlight;

    public void Setup(StickerInstance stickerInstance)
    {
        StickerInstance = stickerInstance;
        _spriteRenderer.sprite = StickerInstance.Data.Sprite;
        _highlight.sprite = StickerInstance.Data.Sprite;
        transform.localPosition = stickerInstance.Position;
    }

    public void Highlight()
    {
        Sequence sequence = Sequence.Create();
        sequence.ChainCallback(() => _highlight.gameObject.SetActive(true));
        sequence.ChainDelay(.8f);
        sequence.ChainCallback(() => _highlight.gameObject.SetActive(false));
    }
}
