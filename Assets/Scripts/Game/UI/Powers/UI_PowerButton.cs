using UnityEngine;
using UnityEngine.UI;

public class UI_PowerButton : MonoBehaviour
{
    [SerializeField] private PowerData _powerData;
    
    [Header("Components")]
    [SerializeField] private Image _powerIcon;
    [SerializeField] private Image _powerLoading;

    public void Setup(PowerData powerData)
    {
        _powerData = powerData;
        _powerIcon.sprite = _powerData.PowerSprite;
    }

    public void UsePower()
    {
        if (_powerData.CurrentLoadTime > 0) return;
        Instantiate(_powerData.PowerBehaviorPrefab);
        _powerData.CurrentLoadTime = _powerData.LoadingTime;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        if (_powerData.CurrentLoadTime >= 0)
        {
            _powerData.CurrentLoadTime -= Time.deltaTime;
            _powerLoading.fillAmount = (float) _powerData.CurrentLoadTime / (float) _powerData.LoadingTime;
            if (_powerLoading.fillAmount < 0) _powerLoading.fillAmount = 0;
        }
    }
}
