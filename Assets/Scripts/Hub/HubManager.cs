using Sirenix.OdinInspector;
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
        SceneManager.LoadScene("Map");
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
}
