using Sirenix.OdinInspector;
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

    public void LaunchNewRun()
    {
        SceneManager.LoadScene("Map");
    }

    public void UseMealTicket(int cost)
    {
        SaveManager.Instance.CurrentSave.MealTickets -= cost;
        // actualize UI 
    }
}
