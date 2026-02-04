using UnityEngine;
using UnityEngine.Events;

public class Interactable_Dialogue : MonoBehaviour
{
    [SerializeField] private string _dialogueNodeName;
    [SerializeField] private UnityEvent _endDialogEvent;

    public void Interact()
    {
        DialogueManager.Instance.DialogueRunner.StartDialogue(_dialogueNodeName);
        DialogueManager.Instance.EndDialogueEvent += CallEndDialogueEvent;
    }

    public void CallEndDialogueEvent()
    {
        _endDialogEvent?.Invoke();
    }
}
