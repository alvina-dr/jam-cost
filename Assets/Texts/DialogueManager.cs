using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using System;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager Instance { get; private set; }

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

    public DialogueRunner DialogueRunner;
    public event Action EndDialogueEvent;

    public void CallEndDialogueEvent()
    {
        EndDialogueEvent?.Invoke();
        EndDialogueEvent = null;
    }
}
