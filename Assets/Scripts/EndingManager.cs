using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private string _endingDialog;


    private void Start()
    {
        DialogueManager.Instance.DialogueRunner.StartDialogue(_endingDialog);
        DialogueManager.Instance.EndDialogueEvent += CallEndDialogueEvent;
        SaveManager.CurrentSave.NumberFirstBossPlayed++;

    }

    public void CallEndDialogueEvent()
    {
        SaveManager.Instance.AddMT(3);
        SceneManager.LoadScene("Hub");
    }

}
