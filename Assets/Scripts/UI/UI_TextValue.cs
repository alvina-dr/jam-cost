using UnityEngine;
using TMPro;
using PrimeTween;

public class UI_TextValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textUI;
    public string GetTextValue() => _tempoText;
    private string _tempoText;

    private Sequence textAnimation;
    private Color _textInitialColor;
    [SerializeField] private SoundData _numberUpSound;

    private void Awake()
    {
        _textInitialColor = _textUI.color;    
    }

    public void SetTextValue(string text, bool animation = true)
    {
        _tempoText = text;
        if (animation)
        {
            textAnimation = Sequence.Create(useUnscaledTime:true);
            textAnimation.Chain(Tween.Scale(transform, 1.1f, .2f));
            textAnimation.ChainCallback(() => _textUI.text = text, warnIfTargetDestroyed:false);
            textAnimation.Chain(Tween.Scale(transform, 1f, .1f));
        }
        else
        {
            _textUI.text = text;
        }
    }

    public void SetTextColor(Color color)
    {
        _textUI.color = color;
    }

    public void ResetTextColor()
    {
        _textUI.color = _textInitialColor;
    }

    public void SetTextValueNumber(int oldNumber, int newNumber, float animationDuration = .5f, bool animation = true)
    {
        _tempoText = $"{newNumber}";
        if (animation)
        {
            textAnimation = Sequence.Create();

            // calculate difference
            int difference = newNumber - oldNumber;
            int addNumber = difference < 0 ? -1 : 1;
            difference = Mathf.Abs(difference);

            int stepNumber = Mathf.Min(difference, 30);

            float numberDuration = animationDuration / (float) stepNumber;
            for (int i = 1; i <= stepNumber; i++)
            {
                int number = oldNumber + Mathf.RoundToInt(addNumber * i * (float) difference / (float) stepNumber);
                int index = i;
                if (number == newNumber)
                {
                    textAnimation.Chain(Tween.Scale(transform, 1.1f, .2f));
                    textAnimation.ChainCallback(() => _textUI.text = $"{newNumber}", warnIfTargetDestroyed: false);
                    textAnimation.Chain(Tween.Scale(transform, 1f, .1f));
                }
                else
                {
                    if (_numberUpSound) textAnimation.ChainCallback(() => AudioManager.PlaySound(_numberUpSound, index * _numberUpSound.PitchBonus));
                    textAnimation.ChainCallback(() => _textUI.text = $"{number}", warnIfTargetDestroyed: false);
                    textAnimation.ChainDelay(numberDuration);
                }
            }

        }
        else
        {
            _textUI.text = $"{newNumber}";
        }
    }

    public void ScaleAnim(float bigSize, float duration)
    {
        Sequence scaleAnim = Sequence.Create();
        scaleAnim.Chain(Tween.Scale(_textUI.transform, bigSize, duration / 2.0f));
        scaleAnim.Chain(Tween.Scale(_textUI.transform, 1, duration / 2.0f));
    }
}
