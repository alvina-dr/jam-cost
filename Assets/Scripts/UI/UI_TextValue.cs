using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_TextValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textUI;
    public string GetTextValue() => _tempoText;
    private string _tempoText;

    public void SetTextValue(string text, bool animation = true)
    {
        _tempoText = text;
        if (animation)
        {
            Sequence textAnimation = DOTween.Sequence();
            textAnimation.Append(transform.DOScale(1.1f, .2f));
            textAnimation.AppendCallback(() => _textUI.text = text);
            textAnimation.Append(transform.DOScale(1f, .1f));
            textAnimation.Play();
        }
        else
        {
            _textUI.text = text;
        }
    }
}
