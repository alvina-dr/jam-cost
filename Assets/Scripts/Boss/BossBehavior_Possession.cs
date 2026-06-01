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
}
