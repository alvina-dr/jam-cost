using PrimeTween;
using UnityEngine;

public class BossBehavior_Possession : MonoBehaviour
{
    #region Singleton
    public static BossBehavior_Possession Instance { get; private set; }

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

    [SerializeField] private SpriteRenderer _lockBoxSpriteRenderer;

    [Header("Second phase screen")]
    [SerializeField] private GameObject _bossPhaseScreen;
    [SerializeField] private Transform _bossPicture;

    private void Start()
    {
        DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_Start");
    }

    public void LockDepositBox()
    {
        _lockBoxSpriteRenderer.gameObject.SetActive(true);
    }

    public void UnlockDepositBox()
    {
        _lockBoxSpriteRenderer.gameObject.SetActive(false);
    }

    public void ShowBossPhaseScreen()
    {
        GameManager.Instance.ScavengingState.CurrentSubState = GS_Scavenging.Scavenging_SubState.RerollCrateAnim;
        _bossPhaseScreen.SetActive(true);
        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(1f);
        sequence.Chain(Tween.Scale(_bossPicture, 1.1f, 0.01f));
        sequence.ChainDelay(1f);
        sequence.Chain(Tween.Scale(_bossPicture, 1.2f, 0.01f));
        sequence.ChainDelay(1f);
        sequence.ChainCallback(() => _bossPhaseScreen.SetActive(false));
        sequence.ChainCallback(() => GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState));
    }
}
