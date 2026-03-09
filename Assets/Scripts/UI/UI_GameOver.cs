using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : UI_Menu
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nodeText;
    [SerializeField] private TextMeshProUGUI _totalTimeText;
    [SerializeField] private TextMeshProUGUI _ppText;

    public override void OpenMenu()
    {
        _scoreText.text = $"{GameManager.Instance.CurrentScore} / {SaveManager.Instance.GetScavengeNode().ScoreGoal}" ;
        _nodeText.text = $"{SaveManager.CurrentSave.CurrentRun.CurrentNode}";
        _totalTimeText.text = $"{DataLoader.Instance.ConvertTimeToMinutes(SaveManager.CurrentSave.CurrentRun.TotalRunDuration)}";
        if (_ppText != null) _ppText.text = $"+{GameManager.Instance.FoundPP}";
        base.OpenMenu();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }

    public void ReloadGame()
    {
        Destroy(SaveManager.Instance.gameObject);
        Destroy(DataLoader.Instance.gameObject);
        SceneManager.LoadScene("Hub");
    }

    public void NextNode()
    {
        GameManager.Instance.SetGameState(GameManager.Instance.RewardState);
    }
}
