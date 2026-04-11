using TMPro;
using UnityEngine;

public class UI_Pause_CombinationEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _combinationName;
    [SerializeField] private TextMeshProUGUI _combinationDescription;

    private CombinationData _combinationData;

    public void Setup(CombinationData combinationData)
    {
        _combinationData = combinationData;
        _combinationName.text = _combinationData.Name;
        _combinationDescription.text = _combinationData.Description;
    }
}
