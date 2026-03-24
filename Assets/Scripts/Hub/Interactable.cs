using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _outlineColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = new Color(_outlineColor.r, _outlineColor.g, _outlineColor.b, 1);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = new Color(_outlineColor.r, _outlineColor.g, _outlineColor.b, 0);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
