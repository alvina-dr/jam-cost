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

    public void SetupActionInput()
    {
        InputAction pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed += Pause;
    }

    public void Pause(InputAction.CallbackContext _)
    {
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
        _menu.CloseMenu();
    }

    private void OnDestroy()
    {
        InputAction pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed -= Pause;
    }
}
