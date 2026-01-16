using extDebug.Menu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class DebugManager : MonoBehaviour
{
    private void Awake()
    {
        DM.Input = new DMCustomInput();
        SetupDebugMenu();
    }

    public void SetupDebugMenu()
    {
        DM.Root.Clear();

        DM.Add("Reload" , action => SceneManager.LoadScene("Game"));
        DM.Add("Win" , action => WinCurrentNode());
        DM.Add("Generate new map" , action => GenerateNewMap());
        DM.Add("Gain/PP" , action => SaveManager.Instance.AddPP(100));
        DM.Add("Gain/MealTickets", action => GainMealTickets());
    }

    public void WinCurrentNode()
    {
        GameManager.Instance?.SetGameState(GameManager.Instance.WinState);
    }

    public void GenerateNewMap()
    {
        SaveManager.Instance.CurrentSave.CurrentDay = 0;
        SaveManager.Instance.CurrentSave.FormerNodeList.Clear();

        SceneManager.LoadScene("Map");
    }

    public void GainMealTickets()
    {
        SaveManager.Instance.CurrentSave.MealTickets += 100;
        HubManager.Instance?.UpdateUpgrades();
    }
}
