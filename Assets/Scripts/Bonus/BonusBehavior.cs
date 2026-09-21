using PrimeTween;
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
