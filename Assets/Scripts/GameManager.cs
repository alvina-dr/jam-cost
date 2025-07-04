using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

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

    public UIManager UIManager;

    public ItemBehavior SelectedItem;
    public float RoundTime;
    [ReadOnly] public float Timer;

    public List<BonusData> BonusList;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!GetTimerPlaying()) return;

        Timer -= Time.deltaTime;
        if (UIManager.Timer.GetTextValue() != Mathf.RoundToInt(Timer).ToString())
        {
            UIManager.Timer.SetTextValue(Mathf.RoundToInt(Timer).ToString());
        }

        if (Timer <= 0)
        {
            EndOfRound();    
        }
    }

    public bool GetTimerPlaying()
    {
        if (UIManager.BonusMenu.Menu.IsOpen()) return false;
        return true;
    }

    public void EndOfRound()
    {
        Time.timeScale = 0f;
        UIManager.TicketMenu.ResetTicket();
        UIManager.BonusMenu.OpenMenu();
    }

    public void ResetTimer()
    {
        Timer = RoundTime;
    }

    public void AddBonus(BonusData bonus)
    {
        BonusList.Add(bonus);
        UIManager.BonusList.UpdateBonusList();
    }
}
