using UnityEngine;
using UnityEngine.UI;

public class UI_PowerButton : MonoBehaviour
{
    [SerializeField] private PowerData _powerData;
    
    [Header("Components")]
    [SerializeField] private Image _powerIcon;
    [SerializeField] private Image _powerOutline;
    [SerializeField] private Image _powerLoading;

    private bool _powerCharged;

    public void Setup(PowerData powerData)
    {
        _powerData = powerData;
        _powerIcon.sprite = _powerData.PowerSprite;
        _powerLoading.fillAmount = (float)_powerData.CurrentLoadTime / (float)_powerData.LoadingTime;
        if (_powerData.CurrentLoadTime <= 0) PowerCharged();
    }

    public void UsePower()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        if (_powerData.CurrentLoadTime > 0) return;

        _powerCharged = false;
        _powerOutline.gameObject.SetActive(false);
        PowerBehavior powerBehavior = Instantiate(_powerData.PowerBehaviorPrefab);
        powerBehavior.transform.position = transform.position;
        _powerData.CurrentLoadTime = _powerData.LoadingTime;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        if (!_powerCharged)
        {
            _powerData.CurrentLoadTime -= Time.deltaTime;
            _powerLoading.fillAmount = (float) _powerData.CurrentLoadTime / (float) _powerData.LoadingTime;
            if (_powerData.CurrentLoadTime <= 0) PowerCharged();
        }
    }

    public void PowerCharged()
    {
        _powerCharged = true;
        _powerLoading.fillAmount = 0;
        _powerOutline.gameObject.SetActive(true);
        // particles when power is charged
        // change color of background of power when power is charged

        if (!SaveManager.CurrentSave.PowerFirstTime)
        {
            SaveManager.CurrentSave.PowerFirstTime = true;
            DialogueManager.Instance.EndDialogueEvent += PlayAgain;
            Time.timeScale = 0;
            DialogueManager.Instance.DialogueRunner.StartDialogue("Onboarding_Powers");
        }
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
    }
}
