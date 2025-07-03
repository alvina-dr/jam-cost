using UnityEngine;

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
    public float Timer;

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            EndOfRound();    
        }
    }

    public void EndOfRound()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        Timer = RoundTime;
    }
}
