using Coffee.UIExtensions;
using UnityEngine;

public class UI_Run : MonoBehaviour
{
    #region Singleton
    public static UI_Run Instance { get; private set; }

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

    public UI_TextValue PPTextValue;
    public UI_TextValue MealTicketTextValue;

    [Header("UI Particle PP")]
    public ParticleSystem UIParticle_PP;
    public RectTransform UIParticle_PP_RectTransform;
    public UIParticleAttractor UIParticle_PP_Attractor;

    [Header("UI Particle MT")]
    public ParticleSystem UIParticle_MT;
    public RectTransform UIParticle_MT_RectTransform;
    public UIParticleAttractor UIParticle_MT_Attractor;
}
