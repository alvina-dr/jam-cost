using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Image _background;
    [SerializeField] private AudioClip _onClickSound;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.DOScale(1f, .3f).SetUpdate(true);
        if (_textMeshProUGUI != null) _textMeshProUGUI.color = Color.black;
        if (_background != null) _background.gameObject.SetActive(false);

    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOScale(1.1f, .3f).SetUpdate(true);
        if (_textMeshProUGUI != null) _textMeshProUGUI.color = Color.white;
        if (_background != null) _background.gameObject.SetActive(true);
    }

    private void OnClick()
    {
        AudioManager.Instance.PlaySFXSound(_onClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(eventData);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
