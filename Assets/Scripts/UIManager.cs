using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HoverPrice HoverPrice;
    public UI_TicketMenu TicketMenu;
    public UI_TextValue Timer;
    public UI_TextValue RoundRemaining;
    public UI_TextValue CoinCount;
    public UI_BonusList BonusList;
    public UI_OverCheck DumpsterOverCheck;
    public UI_OverCheck CoinBagOverCheck;
    public UI_GameOver GameLost;
    public UI_GameOver GameWon;
    public UI_TextPopperManager TextPopperManager_Number;
    public UI_TextPopperManager TextPopperManager_Info;
    public UI_NewHand NewHand;
    public UI_BagMenu BagMenu;
    public UI_PowerMenu PowerMenu;

    [Header("HUD")]
    public UI_TextValue ScoreTextValue;
    public UI_BarValue ScoreBarValue;
}
