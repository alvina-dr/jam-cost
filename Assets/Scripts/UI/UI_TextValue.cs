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

    public void SetTextValueNumber(int oldNumber, int newNumber, bool animation = true)
    {
        _tempoText = $"{newNumber}";
        if (animation)
        {
            textAnimation = Sequence.Create();

            // calculate difference
            int difference = newNumber - oldNumber;
            int addNumber = difference < 0 ? -1 : 1;
            difference = Mathf.Abs(difference);

            float numberDuration = .5f / (float) difference;
            for (int i = 0; i < difference; i++)
            {
                int number = oldNumber + addNumber * (i + 1);

                if (number == newNumber)
                {
                    textAnimation.Chain(Tween.Scale(transform, 1.1f, .2f));
                    textAnimation.ChainCallback(() => _textUI.text = $"{newNumber}", warnIfTargetDestroyed: false);
                    textAnimation.Chain(Tween.Scale(transform, 1f, .1f));
                }
                else
                {
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

    void OnDisable()
    {
        textAnimation.Stop();
    }
}
