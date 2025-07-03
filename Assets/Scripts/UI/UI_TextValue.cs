using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_TextValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textUI;
    public string GetTextValue() => _textUI.text;

    public void SetTextValue(string text, bool animation = true)
    {
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
