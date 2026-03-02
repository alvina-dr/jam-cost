using UnityEngine;
using UnityEngine.Events;

public class Unlockable_Quest : MonoBehaviour
{
    [SerializeField] private QuestData _questForUnlock;
    [SerializeField] private UnityEvent _unlockEvent;

    private void Start()
    {
        UpdateUnlock();
    }

    public void UpdateUnlock()
    {
        if (QuestManager.Instance.QuestDataDictionary.TryGetValue(_questForUnlock.Data.Name, out QuestData questData))
        {
            if (questData.Data.State == QuestData.QuestState.Collected)
            {
                _unlockEvent?.Invoke();
            }
        }
    }
}
