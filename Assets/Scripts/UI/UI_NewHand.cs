using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NewHand : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _panelImage;
    [SerializeField] private AudioClip _newHandSound;

    public void Show()
    {
        _panelImage.transform.localScale = new Vector3(1, 0, 1);
        _text.transform.localPosition = new Vector3(1920, _text.transform.localPosition.y, 0);
        AudioManager.Instance.PlaySFXSound(_newHandSound);

        Sequence showSequence = DOTween.Sequence();
        showSequence.SetUpdate(true);
        showSequence.Append(_canvasGroup.DOFade(1, .3f));
        showSequence.Append(_panelImage.transform.DOScaleY(1, .3f));
        showSequence.Append(_text.transform.DOLocalMoveX(200, .4f).SetEase(Ease.Linear));
        showSequence.Append(_text.transform.DOLocalMoveX(-200, 1f).SetEase(Ease.Linear));
        showSequence.Append(_text.transform.DOLocalMoveX(-1920, .4f).SetEase(Ease.Linear));
        showSequence.Append(_panelImage.transform.DOScaleY(0, .3f));
        showSequence.Append(_canvasGroup.DOFade(0, .3f));
        showSequence.AppendCallback(() => GameManager.Instance.SetGameState(GameManager.GameState.Scavenging));
    }
}
