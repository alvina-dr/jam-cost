using EasyTransition;
using TMPro;
using UnityEngine;

public class UI_GameOver : UI_Menu
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nodeText;
    [SerializeField] private TextMeshProUGUI _totalTimeText;
    [SerializeField] private TextMeshProUGUI _ppText;
    [SerializeField] private TransitionSettings _transitionSettings;

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
        Debug.Log("LOOSE");
        SaveManager.Instance.SaveRun();
        //Destroy(SaveManager.Instance.gameObject);
        Destroy(DataLoader.Instance.gameObject);
        TransitionManager.Instance().TransitionChangeScene("Office", _transitionSettings, 0);
    }

    public void NextNode()
    {
        GameManager.Instance.SetGameState(GameManager.Instance.RewardState);
    }
}
