using UnityEngine;

public class FreeRoundManager : MonoBehaviour
{
    #region Singleton
    public static FreeRoundManager Instance { get; private set; }

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

    [SerializeField] private GameObject _freeRoundInteractable;

    private void Start()
    {
        if (!SaveManager.CurrentSave.FreeRoundFirstTime)
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("FirstFreeRound");
            SaveManager.CurrentSave.FreeRoundFirstTime = true;
        }
    }

    public void LeaveFreeRound()
    {
        if (_freeRoundInteractable.activeSelf)
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("ForgetFreeRound");
            return;
        }

        SaveManager.Instance.NextNode();
    }

    public void GetFreeRound()
    {
        SaveManager.CurrentSave.CurrentRun.RunBonusRound++;

        if (!SaveManager.CurrentSave.GetFreeRoundFirstTime)
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("GetFirstFreeRound");
            SaveManager.CurrentSave.GetFreeRoundFirstTime = true;
        }
    }
}
