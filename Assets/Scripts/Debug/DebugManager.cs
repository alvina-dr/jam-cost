using extDebug.Menu;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

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
    }
}
