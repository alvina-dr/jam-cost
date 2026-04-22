using EasyTransition;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private string _endingDialog;
    [SerializeField] private TransitionSettings _transitionSettings;

    private void Start()
    {
        DialogueManager.Instance.DialogueRunner.StartDialogue(_endingDialog);
        DialogueManager.Instance.EndDialogueEvent += CallEndDialogueEvent;
        SaveManager.CurrentSave.NumberFirstBossPlayed++;
        QuestManager.Instance.CheckQuestCompletionByType<QD_FirstBossNumber>();
    }

    public void CallEndDialogueEvent()
    {
        SaveManager.Instance.AddMT(3);

        TransitionManager.Instance().TransitionChangeScene("Office", _transitionSettings, 0);
    }

}
