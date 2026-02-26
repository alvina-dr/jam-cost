using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HoverPrice HoverPrice;
    public UI_TextValue Timer;
    public UI_TextValue RoundRemaining;
    public UI_TextValue CoinCount;
    public UI_BonusList BonusList;

    [Header("Over check")]
    public UI_OverCheck CrateOverCheck;
    public UI_OverCheck DepotOverCheck;

    [Header("Game Over UI")]
    public UI_GameOver GameLost;
    public UI_GameOver GameWon;

    [Header("Text popper")]
    public UI_TextPopperManager TextPopperManager_Number;
    public UI_TextPopperManager TextPopperManager_Info;
    
    public UI_NewHand NewHand;
    public UI_BagMenu BagMenu;
    public UI_PowerMenu PowerMenu;

    [Header("HUD")]
    public UI_TextValue ScoreTextValue;
    public UI_BarValue ScoreBarValue;
}
