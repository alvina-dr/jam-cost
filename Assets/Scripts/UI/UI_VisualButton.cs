using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_VisualButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _inputHint;
    [SerializeField] private GameObject _selectionIcon;
    [SerializeField] private GameObject _mainIcon;

    [SerializeField] private float _showDuration; 
    [SerializeField] private float _hideDuration;

    private bool IsSelected { get { return EventSystem.current.currentSelectedGameObject == gameObject; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            Hide(true);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Show(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Hide(true);
    }

    public void Show(bool animated)
    {
        if (animated)
        {
            Sequence show = DOTween.Sequence().SetLink(gameObject).SetUpdate(true);

            show.Insert(0, _inputHint.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), _showDuration));
            show.Insert(0, _selectionIcon.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), _showDuration));
            show.Insert(0, _mainIcon.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), _showDuration));
            show.Play();
        }
        else
        {
            _inputHint.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _selectionIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _mainIcon.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }
    }

    public void Hide(bool animated)
    {
        if (animated)
        {
            Sequence hide = DOTween.Sequence().SetLink(gameObject).SetUpdate(true);

            hide.Insert(0, _inputHint.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), _hideDuration));
            hide.Insert(0, _selectionIcon.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), _hideDuration));
            hide.Insert(0, _mainIcon.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), _showDuration));
            hide.Play();
        }
        else
        {
            _inputHint.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            _selectionIcon.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            _mainIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
