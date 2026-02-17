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

    public GameObject BreakRoom;
    public GameObject InsideLocker;
    [SerializeField] private List<HubUpgrade> _hubUpgradeList = new();
    public UI_PowerShop PowerShop;
    public UI_PermanentBonusShop PermanentBonusShop;

    [Button]
    public void UpdateUpgradeList()
    {
        _hubUpgradeList.Clear();
        _hubUpgradeList = FindObjectsByType<HubUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    [Button]
    public void OpenLocker()
    {
        BreakRoom.SetActive(false);
        InsideLocker.SetActive(true);
    }

    [Button]
    public void CloseLocker()
    {
        BreakRoom.SetActive(true);
        InsideLocker.SetActive(false);
    }

    public void OpenPowerShop()
    {
        PowerShop.OpenMenu();
    }

    public void OpenPermanentBonusShop()
    {
        PermanentBonusShop.OpenMenu();
    }

    public void LaunchNewRun()
    {
        SceneManager.LoadScene("Map");
        SaveManager.Instance.StartNewRun();
    }

    public void UseMealTicket(int cost)
    {
        SaveManager.CurrentSave.MealTickets -= cost;
        UpdateUpgrades();
        // actualize UI 
    }

    public void UpdateUpgrades()
    {
        for (int i = 0; i < _hubUpgradeList.Count; i++)
        {
            _hubUpgradeList[i].UpdatePrice();
        }
    }
}
