using DG.Tweening;
using UnityEngine;

public class UI_DialogMenu : MonoBehaviour
{
    [SerializeField] private UI_DialogBubble _dialogBubbleLeft;
    [SerializeField] private UI_DialogBubble _dialogBubbleRight;
    public DialogData CurrentDialogData;
    [SerializeField] private Transform _dialogBubbleParent;

    private void Start()
    {
        Open();
    }

    public void Open()
    {
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
    }
}
