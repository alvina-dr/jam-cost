using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private TransitionSettings _transitionSettings; 

    public void StartGame()
    {
        SaveManager.Instance.LoadOrCreateSave();
        //if (SceneManager.GetSceneByName(SaveManager.CurrentSave.LastSceneName) != null) SceneManager.LoadScene(SaveManager.CurrentSave.LastSceneName);
        //else SceneManager.LoadScene("Hub");

        if (SaveManager.CurrentSave.SeeOnboarding == true)
        {
            TransitionManager.Instance().TransitionChangeScene("Hub", _transitionSettings, 0);
        }
        else
        {
            TransitionManager.Instance().TransitionChangeScene("Onboarding", _transitionSettings, 0);
        }
    }

    public void EraseSave()
    {
        SaveManager.Instance.EraseSave();
    }
}
