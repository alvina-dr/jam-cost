using Coffee.UIExtensions;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HoverPrice HoverPrice;
    public UI_TextValue Timer;
    public UI_TextValue RoundRemaining;

    [Header("Game Over UI")]
    public UI_GameOver GameLost;
    public UI_GameOver GameWon;

    [Header("Text popper")]
    public UI_TextPopperManager TextPopperManager_Number;
    public UI_TextPopperManager TextPopperManager_Info;
    
    public UI_NewHand NewHand;
    public UI_BagMenu BagMenu;
    public UI_PowerMenu PowerMenu;
    public UI_Menu RewardMenu;

    [Header("HUD")]
    public UI_TextValue ScoreTextValue;
    public UI_BarValue ScoreBarValue;

    [Header("Time particles")]
    public ParticleSystem UIParticle_Time;
    public RectTransform UIParticle_Time_RectTransform;
    public UIParticleAttractor UIParticle_Time_Attractor;

    public void AddTimer(int number, Vector3 worldPosition)
    {
        if (number > 0)
        {
            UIParticle_Time_Attractor.onAttracted.RemoveAllListeners();
            UIParticle_Time_Attractor.onAttracted.AddListener(() => OnParticleAttracted_Time(1));
            UIParticle_Time_Attractor.onAttracted.AddListener(() => UIParticle_Time.TriggerSubEmitter(0));
            UIParticle_Time_RectTransform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            UIParticle_Time_RectTransform.localPosition = new Vector3(UIParticle_Time_RectTransform.localPosition.x, UIParticle_Time_RectTransform.localPosition.y, 0);
            UIParticle_Time.Emit(number);
        }
        else
        {
            OnParticleAttracted_Time(number);
        }
    }

    public void OnParticleAttracted_Time(int number)
    {
        GameManager.Instance.ScavengingState.Timer += number;
    }
}
