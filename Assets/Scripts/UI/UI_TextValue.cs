using UnityEngine;
using TMPro;
using PrimeTween;

public class UI_TextValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textUI;
    public string GetTextValue() => _tempoText;
    private string _tempoText;

    private Sequence textAnimation;

    public void SetTextValue(string text, bool animation = true)
    {
        _tempoText = text;
        if (animation)
        {
            textAnimation = Sequence.Create();
            textAnimation.Chain(Tween.Scale(transform, 1.1f, .2f));
            textAnimation.ChainCallback(() => _textUI.text = text, warnIfTargetDestroyed:false);
            textAnimation.Chain(Tween.Scale(transform, 1f, .1f));
        }
        else
        {
            _textUI.text = text;
        }
    }

    void OnDisable()
    {
        textAnimation.Stop();
    }
}
