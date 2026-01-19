using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_BarValue : MonoBehaviour
{
    #region Properties
    [SerializeField] private Slider bar;
    #endregion

    #region Methods
    public void SetBarValue(float _currentValue, float _maxValue, bool animate = true)
    {
        if (animate)
        {
            bar.DOValue(_currentValue / _maxValue, .2f);
        }
        else
        {
            bar.value = _currentValue / _maxValue;
        }
    }
    #endregion
}
