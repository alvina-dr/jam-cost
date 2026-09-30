using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _outlineColor;

    private void OnMouseEnter()
    {
        Color color = new Color(_outlineColor.r, _outlineColor.g, _outlineColor.b, 1);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }

    private void OnMouseExit()
    {
        Color color = new Color(_outlineColor.r, _outlineColor.g, _outlineColor.b, 0);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
