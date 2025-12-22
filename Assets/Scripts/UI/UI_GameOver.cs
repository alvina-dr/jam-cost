using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    public UI_Menu Menu;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Open()
    {
        _scoreText.text = GameManager.Instance.CurrentScore.ToString() + "$";
        Menu.OpenMenu();
    }

    public void ReloadGame()
    {
        Destroy(SaveManager.Instance.gameObject);
        Destroy(DataLoader.Instance.gameObject);
        SceneManager.LoadScene("Hub");
    }

    public void NextDay()
    {
        SaveManager.Instance.NextDay();
    }
}
