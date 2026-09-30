using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnClick;
    [SerializeField] private Image _image;

    private void Start()
    {
        if (_image) _image.alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        _image.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.white;
    }
}
