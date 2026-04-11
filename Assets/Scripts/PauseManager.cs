using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    #region Singleton
    public static PauseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            SetupActionInput();
        }
    }
    #endregion

    public bool IsPaused { get; private set; }

    [SerializeField] private UI_Menu _menu;
    [SerializeField] private List<UI_Menu> _menuList = new();

    public void SetupActionInput()
    {
        InputAction pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed += Pause;
    }

    public void ContinueButton()
    {
        Pause(new InputAction.CallbackContext());
    }

    public void Pause(InputAction.CallbackContext _)
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState
            && GameManager.Instance.CurrentGameState != GameManager.Instance.PreparationState
            && GameManager.Instance.CurrentGameState != null
            && GameManager.Instance.CurrentGameState != GameManager.Instance.RewardState) return;

        IsPaused = !IsPaused;
        if (IsPaused)
        {
            Time.timeScale = 0f;
            OpenMenu();
        }
        else
        {
            Time.timeScale = 1f;
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        _menu.OpenMenu();
    }

    public void CloseMenu()
    {
        CloseAllMenusExcept(null);
        _menu.CloseMenu();
    }

    public void CloseAllMenusExcept(UI_Menu exceptionMenu)
    {
        for (int i = 0; i < _menuList.Count; i++)
        {
            if(_menuList[i] != exceptionMenu) _menuList[i].CloseMenu();
        }
    }

    private void OnDestroy()
    {
        InputAction pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed -= Pause;
    }
}
