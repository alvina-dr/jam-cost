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
    [SerializeField] private GameObject _bossPhaseScreen;

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
        sequence.ChainDelay(3f);
        sequence.ChainCallback(() => _bossPhaseScreen.SetActive(false));
        sequence.ChainCallback(() => GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState));
    }
}
