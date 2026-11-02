using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using PrimeTween;

public class UI_Combination : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CombinationData _combinationData;
    [SerializeField] private TextMeshProUGUI _combinationName;

    public void Setup(CombinationData combinationData)
    {
        gameObject.SetActive(true);
        _combinationData = combinationData;
        _combinationName.SetText(_combinationData.Data.Name);
        Sequence showSequence = Sequence.Create();
        showSequence.Chain(Tween.Scale(transform, 1.3f, .05f));
        showSequence.Chain(Tween.Scale(transform, 1, .1f));
    }


    public void Reset()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(_combinationData, transform.position, Vector3.zero);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
