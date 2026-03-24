using UnityEngine;
using UnityEngine.Events;

public class Interactable_Dialogue : Interactable
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
