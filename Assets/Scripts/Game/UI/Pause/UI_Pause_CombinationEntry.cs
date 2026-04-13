using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Pause_CombinationEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _combinationName;
    [SerializeField] private TextMeshProUGUI _combinationDescription;
    [SerializeField] private Transform _newIndicator;

    private CombinationData _combinationData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_combinationData.Data.State == CombinationData.CombinationDiscovery.New)
        {
            _combinationData.SeeDiscoveredCombination();
            _newIndicator.gameObject.SetActive(false);
        }
     }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void Setup(CombinationData combinationData)
    {
        _combinationData = combinationData;
        switch (_combinationData.Data.State)
        {
            case CombinationData.CombinationDiscovery.Undiscovered:
                _combinationName.text = "???";
                _combinationDescription.gameObject.SetActive(false);
                _newIndicator.gameObject.SetActive(false);
                break;
            case CombinationData.CombinationDiscovery.New:
                _combinationName.text = _combinationData.Data.Name;
                _combinationDescription.text = _combinationData.Description;
                _combinationDescription.gameObject.SetActive(true);
                _newIndicator.gameObject.SetActive(true);
                break;
            case CombinationData.CombinationDiscovery.Discovered:
                _combinationName.text = _combinationData.Data.Name;
                _combinationDescription.text = _combinationData.Description;
                _combinationDescription.gameObject.SetActive(true);
                _newIndicator.gameObject.SetActive(false);
                break;
        }
        

    }
}
