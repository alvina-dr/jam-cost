using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnboardingManager : MonoBehaviour
{
    #region Singleton
    public static OnboardingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [TextArea, SerializeField] private List<string> _stringList;
    private int _currentIndex = 0;

    [Header("Buttons")]
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _previousButton;
    [SerializeField] private GameObject _startButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _text;


    private void Start()
    {
        Setup();
    }

    public void Next()
    {
        _currentIndex++;

        Setup();
    }

    public void Previous()
    {
        _currentIndex--;
        
        Setup();
    }

    public void Setup()
    {
        _text.text = _stringList[_currentIndex];

        if (_currentIndex == _stringList.Count - 1)
        {
            _nextButton.gameObject.SetActive(false);
            _startButton.gameObject.SetActive(true);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
            _startButton.gameObject.SetActive(false);
        }

        if (_currentIndex == 0)
        {
            _previousButton.gameObject.SetActive(false);
        }
        else
        {
            _previousButton.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        SaveManager.CurrentSave.SeeOnboarding = true;
        SceneManager.LoadScene("Hub");
    }
}
