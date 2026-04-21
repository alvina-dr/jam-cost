using EasyTransition;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    #region Singleton
    public static HubManager Instance { get; private set; }

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

    public UI_PowerShop PowerShop;
    public UI_PermanentBonusShop PermanentBonusShop;
    public UI_QuestsMenu QuestsMenu;

    [SerializeField] private Transform _questBoardClue;

    [SerializeField] private TransitionSettings _transitionSettings;

    private void Start()
    {
        if (!SaveManager.CurrentSave.HubFirstTime && SceneManager.GetActiveScene().name == "Hub")
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("NPC1_Introduction");
            SaveManager.CurrentSave.HubFirstTime = true;
        }

        ShowQuestBoardIndication();
    }

    public void OpenPowerShop()
    {
        PowerShop.OpenMenu();
    }

    public void OpenPermanentBonusShop()
    {
        PermanentBonusShop.OpenMenu();
    }

    public void OpenQuestMenu()
    {
        QuestsMenu.OpenMenu();
    }

    public void LaunchNewRun()
    {
        TransitionManager.Instance().TransitionChangeScene("Game", _transitionSettings, 0);
        SaveManager.Instance.StartNewRun();
    }

    public void UpdateAllUnlocks()
    {
        List<Unlockable_Quest> unlockableList = FindObjectsByType<Unlockable_Quest>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        for (int i = 0; i < unlockableList.Count; i++)
        {
            unlockableList[i].UpdateUnlock();
        }
    }

    public void ShowQuestBoardIndication()
    {
        List<QuestData> questDataList = QuestManager.Instance.QuestDataDictionary.Values.ToList();
        if (_questBoardClue) _questBoardClue.gameObject.SetActive(questDataList.Find(x => x.Data.State == QuestData.QuestState.WaitCollection || x.Data.State == QuestData.QuestState.New));
    }

    public void GoToBreakroom()
    {
        TransitionManager.Instance().TransitionChangeScene("Hub", _transitionSettings, 0);
    }

    public void GoToOffice()
    {
        TransitionManager.Instance().TransitionChangeScene("Office", _transitionSettings, 0);
    }
}
