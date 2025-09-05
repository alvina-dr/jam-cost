using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class UI_DialogMenu : MonoBehaviour
{
    public UI_Menu Menu;
    [SerializeField] private UI_DialogBubble _dialogBubbleLeft;
    [SerializeField] private UI_DialogBubble _dialogBubbleRight;
    public DialogData CurrentDialogData;
    [SerializeField] private Transform _dialogBubbleParent;
    [SerializeField] private Transform _continueButton;
    [SerializeField] private AudioClip _buttonApparitionSound;

    public void Open()
    {
        Menu.OpenMenu();
        Sequence showSequence = DOTween.Sequence();
        for (int i = 0; i < CurrentDialogData.LineDataList.Count; i++)
        {
            int index = i;
            showSequence.AppendCallback(() =>
            {
                UI_DialogBubble dialogBubble;
                if (CurrentDialogData.LineDataList[index].DialogSide == DialogData.LineData.Side.Left)
                    dialogBubble = Instantiate(_dialogBubbleLeft, _dialogBubbleParent);
                else
                    dialogBubble = Instantiate(_dialogBubbleRight, _dialogBubbleParent);
                dialogBubble.Setup(CurrentDialogData.LineDataList[index]);
            });
            showSequence.AppendInterval(CurrentDialogData.LineDataList[index].Interval);
        }
        showSequence.AppendCallback(() => _continueButton.parent = _dialogBubbleParent);
        showSequence.AppendCallback(() => AudioManager.Instance.PlaySFXSound(_buttonApparitionSound));
        showSequence.AppendCallback(() => _continueButton.gameObject.SetActive(true));
        showSequence.AppendCallback(() => PlayerPrefs.SetString(CurrentDialogData.name, CurrentDialogData.name));
    }

    public void Close()
    {
        Menu.CloseMenu();
        GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState);
    }

    public bool HasBeenPlayed()
    {
        if (PlayerPrefs.HasKey(CurrentDialogData.name))
        {
            return true;
        }
        return false;
    }
}
