using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class BonusBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BonusData BonusData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Setup(BonusData bonusData)
    {
        gameObject.SetActive(true);
        BonusData = bonusData;
        _spriteRenderer.sprite = BonusData.Icon;
    }

    [Button]
    public void Highlight()
    {
        Color outlineColor = _spriteRenderer.material.GetColor("_OutlineColor");
        _spriteRenderer.material.SetColor("_OutlineColor", Color.white);
        GameManager.Instance.UIManager.TextPopperManager_Info.PopText(BonusData.Name, transform.position + Vector3.up * 1.5f);

        Sequence highlight = Sequence.Create();
        highlight.Chain(Tween.Scale(transform, 1.4f, .2f));
        highlight.Group(Tween.ShakeLocalRotation(_spriteRenderer.transform, new Vector3(0, 0, 20), .2f));
        highlight.Group(Tween.LocalPositionY(_spriteRenderer.transform, .3f, .2f));
        highlight.ChainDelay(.2f);
        highlight.Chain(Tween.Scale(transform, 1f, .1f));
        highlight.Group(Tween.LocalPositionY(_spriteRenderer.transform, 0, .1f));

        highlight.ChainCallback(() => _spriteRenderer.material.SetColor("_OutlineColor", outlineColor));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tween.Scale(transform, 1.2f, .2f);
        Tween.ShakeLocalRotation(_spriteRenderer.transform, new Vector3(0, 0, 10), .2f);
        Tween.LocalPositionY(_spriteRenderer.transform, .3f, .2f);
        TooltipManager.Instance.ShowTooltip(BonusData, transform.position, Vector3.up * 200);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tween.Scale(transform, 1, .2f);
        Tween.ShakeLocalRotation(_spriteRenderer.transform, new Vector3(0, 0, 10), .2f);
        Tween.LocalPositionY(_spriteRenderer.transform, 0, .2f);
        TooltipManager.Instance.HideTooltip();
    }
}
