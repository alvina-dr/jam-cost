using UnityEngine;
using UnityEngine.UI;

public class UI_PowerButton : MonoBehaviour
{
    [SerializeField] private PowerData _powerData;
    
    [Header("Components")]
    [SerializeField] private Image _powerIcon;
    [SerializeField] private Image _powerLoading;

    private float _timer;


    public void Setup(PowerData powerData)
    {
        _powerData = powerData;
        _powerIcon.sprite = _powerData.PowerSprite;
    }

    public void UsePower()
    {
        if (_timer > 0) return;
        Instantiate(_powerData.PowerBehaviorPrefab);
        _timer = _powerData.LoadingTime;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        if (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            _powerLoading.fillAmount = (float) _timer / (float) _powerData.LoadingTime;
            if (_powerLoading.fillAmount < 0) _powerLoading.fillAmount = 0;
        }
    }
}
