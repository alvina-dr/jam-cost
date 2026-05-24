using EasyTransition;
using UnityEngine;

public class PossessionRoomManager : MonoBehaviour
{
    [SerializeField] private TransitionSettings _transitionSettings;
    [SerializeField] private MapNodeData _bossMapNode;

    private void Start()
    {
        SaveManager.Instance.LoadYarnState();
        DialogueManager.Instance.EndDialogueEvent += StartBoss;
        DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_Room_Encounter");
        SaveManager.CurrentSave.NumberFirstBossPlayed++;
    }

    public void StartBoss()
    {
        SaveManager.Instance.CurrentMapNode = Instantiate(_bossMapNode);
        SaveManager.Instance.ChangeScene("Game", _transitionSettings, 0);
    }
}
