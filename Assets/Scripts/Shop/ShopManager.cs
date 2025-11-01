using Sirenix.OdinInspector;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager Instance { get; private set; }

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

    [ReadOnly] public GameState CurrentGameState;

    [Header("States")]
    public GS_ChoosingBonus ChoosingBonus;

    [Header("References")]
    public UIManager UIManager;

    private void Start()
    {
        CurrentGameState = ChoosingBonus;
        CurrentGameState.EnterState();
    }

    public void SetGameState(GameState newState)
    {
        CurrentGameState.ExitState();
        CurrentGameState = newState;
        CurrentGameState.EnterState();
    }

    public void AddBonus(BonusData bonus)
    {
        SaveManager.Instance.CurrentSave.BonusList.Add(bonus);
    }
}
