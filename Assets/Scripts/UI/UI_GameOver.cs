using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    public UI_Menu Menu;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nodeText;
    [SerializeField] private TextMeshProUGUI _totalTimeText;
    [SerializeField] private TextMeshProUGUI _ppText;

    public void Open()
    {
        _scoreText.text = $"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}" ;
        _nodeText.text = $"{SaveManager.CurrentSave.CurrentRun.CurrentNode}";
        _totalTimeText.text = $"{SaveManager.CurrentSave.CurrentRun.TotalRunDuration}";
        if (_ppText != null) _ppText.text = $"+{GameManager.Instance.FoundPP}";
        Menu.OpenMenu();
    }

    public void ReloadGame()
    {
        Destroy(SaveManager.Instance.gameObject);
        Destroy(DataLoader.Instance.gameObject);
        SceneManager.LoadScene("Hub");
    }

    public void NextNode()
    {
        SaveManager.Instance.NextNode();
    }
}
