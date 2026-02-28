using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SaveManager.Instance.LoadOrCreateSave();
        //if (SceneManager.GetSceneByName(SaveManager.CurrentSave.LastSceneName) != null) SceneManager.LoadScene(SaveManager.CurrentSave.LastSceneName);
        //else SceneManager.LoadScene("Hub");

        if (SaveManager.CurrentSave.SeeOnboarding == true)
        {
            SceneManager.LoadScene("Hub");
        }
        else
        {
            SceneManager.LoadScene("Onboarding");
        }
    }

    public void EraseSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
