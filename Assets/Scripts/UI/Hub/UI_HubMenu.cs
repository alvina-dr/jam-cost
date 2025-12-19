using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_HubMenu : MonoBehaviour
{
    public void LaunchNewRun()
    {
        SceneManager.LoadScene("Map");
    }
}
