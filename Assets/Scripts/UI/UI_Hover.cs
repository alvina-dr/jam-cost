using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent _onStartHover;
    public UnityEvent _onEndHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onStartHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onEndHover?.Invoke();
    }
}
