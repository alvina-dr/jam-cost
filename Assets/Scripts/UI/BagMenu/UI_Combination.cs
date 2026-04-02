using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using PrimeTween;
using System.Collections;

public class UI_Combination : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CombinationData _combinationData;
    [SerializeField] private TextMeshProUGUI _combinationName;

    public void Setup(CombinationData combinationData)
    {
        gameObject.SetActive(true);
        _combinationData = combinationData;
        _combinationName.SetText(_combinationData.Name);
        StartCoroutine(Rebuild());
        Sequence showSequence = Sequence.Create();
        showSequence.Chain(Tween.Scale(transform, 1.3f, .05f));
        showSequence.Chain(Tween.Scale(transform, 1, .1f));
    }

    IEnumerator Rebuild()
    {
        _combinationName.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        _combinationName.gameObject.SetActive(true);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(_combinationData, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
