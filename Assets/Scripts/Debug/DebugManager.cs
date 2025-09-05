using extDebug.Menu;
using UnityEngine;
using UnityEditor.SceneManagement;
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
        DM.Add("Test" , action => Debug.Log("test"));
        DM.Add("Win" , action => WinCurrentNode());
    }

    public void WinCurrentNode()
    {
        //GameManager.Instance.SetGameState(GameState.ChoosingBonus); � remettre
    }
}
