using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Markup;
using Yarn.Unity;
using Yarn.Unity.Attributes;
using System.Linq;
using System;
using PrimeTween;
#nullable enable

public class LineDialogBubblePresenter : DialoguePresenterBase
{
    internal enum TypewriterType
    {
        Instant, ByLetter, ByWord, Custom,
    }

    /// <summary>
    /// The canvas group that contains the UI elements used by this Line
    /// View.
    /// </summary>
    /// <remarks>
    [Space]
    [MustNotBeNull]
    public CanvasGroup? canvasGroup;

    public RectTransform dialogBubble;
    public RectTransform dialogBubblePoint;
    private Vector3 _startingPositionDialogBubblePoint;

    [Header("Offset")]
    public Vector3 screenBorderOffset;
    public Vector3 dialogBubbleOffset;

    /// <summary>
    /// The <see cref="TMP_Text"/> object that displays the text of
    /// dialogue lines.
    /// </summary>
    [MustNotBeNull]
    public TMP_Text? lineText;

    /// <summary>
    /// Controls whether this Line View will automatically to the Dialogue
    /// Runner that the line is complete as soon as the line has finished
    /// appearing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this value is true, the Line View will 
    /// </para>
    /// <para style="note"><para>The <see cref="DialogueRunner"/> will not
    /// proceed to the next piece of content (e.g. the next line, or the
    /// next options) until all Dialogue Presenters have reported that they have
    /// finished presenting their lines. If a <see cref="LinePresenter"/>
    /// doesn't report that it's finished until it receives input, the <see
    /// cref="DialogueRunner"/> will end up pausing.</para>
    /// <para>
    /// This is useful for games in which you want the player to be able to
    /// read lines of dialogue at their own pace, and give them control over
    /// when to advance to the next line.</para></para>
    /// </remarks>
    [Group("Automatically Advance Dialogue")]
    public bool autoAdvance = false;

    /// <summary>
    /// The amount of time after the line finishes appearing before
    /// automatically ending the line, in seconds.
    /// </summary>
    /// <remarks>This value is only used when <see cref="autoAdvance"/> is
    /// <see langword="true"/>.</remarks>
    [Group("Automatically Advance Dialogue")]
    [ShowIf(nameof(autoAdvance))]
    [Label("Delay Before Advancing")]
    public float autoAdvanceDelay = 1f;


    // typewriter fields

    [Group("Typewriter")]
    [SerializeField] internal TypewriterType typewriterStyle = TypewriterType.ByLetter;

    /// <summary>
    /// The number of characters per second that should appear during a
    /// typewriter effect.
    /// </summary>
    [Group("Typewriter")]
    [ShowIf(nameof(typewriterStyle), TypewriterType.ByLetter)]
    [Label("Letters per Second")]
    [Min(0)]
    public int lettersPerSecond = 60;

    [Group("Typewriter")]
    [ShowIf(nameof(typewriterStyle), TypewriterType.ByWord)]
    [Label("Words per Second")]
    [Min(0)]
    public int wordsPerSecond = 10;

    [Group("Typewriter")]
    [ShowIf(nameof(typewriterStyle), TypewriterType.Custom)]
    [UnityEngine.Serialization.FormerlySerializedAs("CustomTypewriter")]
    [MustNotBeNull("Attach a component that implements the " + nameof(IAsyncTypewriter) + " interface.")]
    public UnityEngine.Object? customTypewriter;

    private PrimeTween.Sequence _sequence;

    /// <summary>
    /// A list of <see cref="ActionMarkupHandler"/> objects that will be
    /// used to handle markers in the line.
    /// </summary>
    [Group("Typewriter")]
    [Label("Event Handlers")]
    [UnityEngine.Serialization.FormerlySerializedAs("actionMarkupHandlers")]
    [SerializeField] List<ActionMarkupHandler> eventHandlers = new List<ActionMarkupHandler>();
    private List<IActionMarkupHandler> ActionMarkupHandlers
    {
        get
        {
            var pauser = new PauseEventProcessor();
            List<IActionMarkupHandler> ActionMarkupHandlers = new()
            {
                pauser,
            };
            ActionMarkupHandlers.AddRange(eventHandlers);
            return ActionMarkupHandlers;
        }
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        HideDialogBubble();
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueStartedAsync()
    {
        HideDialogBubble();
        return YarnTask.CompletedTask;
    }

    private void Awake()
    {
        _startingPositionDialogBubblePoint = dialogBubblePoint.transform.localPosition;
        switch (typewriterStyle)
        {
            case TypewriterType.Instant:
                Typewriter = new InstantTypewriter()
                {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                };
                break;

            case TypewriterType.ByLetter:
                Typewriter = new LetterTypewriter()
                {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                    CharactersPerSecond = this.lettersPerSecond,
                };
                break;

            case TypewriterType.ByWord:
                Typewriter = new WordTypewriter()
                {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                    WordsPerSecond = this.wordsPerSecond,
                };
                break;

            case TypewriterType.Custom:
                Typewriter = new CustomTypewriter()
                {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                    CharactersPerSecond = this.lettersPerSecond,
                };
                //Typewriter = ValidateCustomTypewriter();
                //Typewriter?.ActionMarkupHandlers.AddRange(ActionMarkupHandlers);
                //if (Typewriter == null)
                //{
                //    Debug.LogWarning("Typewriter mode is set to custom but there is no typewriter set.");
                //}
                //else
                //{

                //}
                break;
        }
    }

    void OnValidate()
    {
        var tw = ValidateCustomTypewriter();
        if (tw == null)
        {
            customTypewriter = null;
        }
        else
        {
            customTypewriter = tw as Component;
        }
    }

    private IAsyncTypewriter? ValidateCustomTypewriter()
    {
        if (customTypewriter is GameObject gameObject)
        {
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component is IAsyncTypewriter)
                {
                    customTypewriter = component;
                    return component as IAsyncTypewriter;
                }
            }
        }

        if (customTypewriter is Component)
        {
            if (customTypewriter is IAsyncTypewriter)
            {
                return customTypewriter as IAsyncTypewriter;
            }
        }

        return null;
    }

    /// <summary>Presents a line using the configured text view.</summary>
    /// <inheritdoc cref="DialoguePresenterBase.RunLineAsync(LocalizedLine, LineCancellationToken)" path="/param"/>
    /// <inheritdoc cref="DialoguePresenterBase.RunLineAsync(LocalizedLine, LineCancellationToken)" path="/returns"/>
    public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
    {
        if (lineText == null)
        {
            Debug.LogError($"{nameof(LinePresenter)} does not have a text view. Skipping line {line.TextID} (\"{line.RawText}\")");
            return;
        }

        MarkupParseResult text;

        // configuring the text fields

        text = line.TextWithoutCharacterName;

        // ADDED BY ALVINA FOR SPEECH BUBBLE SYSTEM
        // show dialog bubble above matching character

        List<DialogBubbleCharacter> dialogBubbleCharacterList = FindObjectsByType<DialogBubbleCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        DialogBubbleCharacter dialogBubbleCharacter = null;
        for (int i = 0; i < dialogBubbleCharacterList.Count; i++)
        {
            if (dialogBubbleCharacterList[i].CharacterName == line.CharacterName)
            {
                dialogBubbleCharacter = dialogBubbleCharacterList[i];
                break;
            }
        }

        if (dialogBubbleCharacter != null)
        {
            PlaceDialogBubble(Camera.main.WorldToScreenPoint(dialogBubbleCharacter.transform.position));
        }
        // END ADDED BY ALVINA FOR SPEECH BUBBLE SYSTEM END

        Typewriter ??= new InstantTypewriter()
        {
            ActionMarkupHandlers = this.ActionMarkupHandlers,
            Text = this.lineText,
        };

        Typewriter.PrepareForContent(text);

        ShowDialogBubble();

        await Typewriter.RunTypewriter(text, token.HurryUpToken).SuppressCancellationThrow();

        // if we are set to autoadvance how long do we hold for before continuing?
        if (autoAdvance)
        {
            await YarnTask.Delay((int)(autoAdvanceDelay * 1000), token.NextContentToken).SuppressCancellationThrow();
        }
        else
        {
            await YarnTask.WaitUntilCanceled(token.NextContentToken).SuppressCancellationThrow();
        }

        Typewriter.ContentWillDismiss();

        HideDialogBubble();
    }

    private void ShowDialogBubble(Action callback = null)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            _sequence.Stop();
            _sequence = Sequence.Create(useUnscaledTime: true);
            _sequence.timeScale = 1.0f;
            _sequence.ChainDelay(.15f);
            _sequence.Chain(Tween.Scale(dialogBubble.transform, 1.1f, .1f));
            _sequence.Chain(Tween.Scale(dialogBubble.transform, 1f, .05f));
            _sequence.ChainCallback(() => callback?.Invoke());
        }
    }

    private void HideDialogBubble(Action callback = null)
    {
        if (canvasGroup != null)
        {
            _sequence.Stop();
            _sequence = Sequence.Create(useUnscaledTime:true);
            _sequence.Chain(Tween.Scale(dialogBubble.transform, 1.1f, .1f));
            _sequence.Chain(Tween.Scale(dialogBubble.transform, 0f, .05f));
            _sequence.ChainCallback(() =>
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                callback?.Invoke();
            });
        }
    }

    private void PlaceDialogBubble(Vector3 targetPosition)
    {
        targetPosition += dialogBubbleOffset;
        Vector3 newPosition = targetPosition;

        // TOO FAR LEFT
        if (newPosition.x - dialogBubble.sizeDelta.x / 2 < screenBorderOffset.x)
        {
            newPosition -= new Vector3(newPosition.x - dialogBubble.sizeDelta.x / 2 - screenBorderOffset.x, 0, 0);
        }

        // TOO FAR RIGHT
        if (newPosition.x + dialogBubble.sizeDelta.x / 2 > Screen.width - screenBorderOffset.x)
        {
            newPosition -= new Vector3((newPosition.x + dialogBubble.sizeDelta.x / 2) - (Screen.width - screenBorderOffset.x), 0, 0);
        }

        // UNDER BOTTOM
        if (newPosition.y - dialogBubble.sizeDelta.y / 2 < screenBorderOffset.y)
        {
            //newPosition -= new Vector3(0, newPosition.y - dialogBubble.sizeDelta.y / 2 - screenBorderOffset.y, 0);
        }

        // ABOVE TOP
        Debug.Log("position : " + newPosition);
        Debug.Log("screen height min : " + (Screen.height - screenBorderOffset.y));
        if (newPosition.y + dialogBubble.sizeDelta.y  > Screen.height - screenBorderOffset.y)
        {
            Debug.Log("above top");
            dialogBubble.pivot = new Vector2(dialogBubble.pivot.x, 1);
            dialogBubblePoint.anchorMin = new Vector2(dialogBubble.anchorMin.x, 1);
            dialogBubblePoint.anchorMax = new Vector2(dialogBubble.anchorMax.x, 1);
            dialogBubblePoint.transform.localPosition = new Vector3(dialogBubble.pivot.x, 10, 0);
            dialogBubblePoint.localScale = new Vector3(1, -1, 1);
            newPosition -= new Vector3(0, dialogBubbleOffset.y * 2, 0);
            //newPosition -= new Vector3(0, (newPosition.y + dialogBubble.sizeDelta.y / 2) - (Screen.height - screenBorderOffset.y), 0);
        }
        else
        {
            dialogBubble.pivot = new Vector2(dialogBubble.pivot.x, 0);
            dialogBubblePoint.anchorMin = new Vector2(dialogBubble.anchorMin.x, 0);
            dialogBubblePoint.anchorMax = new Vector2(dialogBubble.anchorMax.x, 0);
            dialogBubblePoint.transform.localPosition = new Vector3(dialogBubble.pivot.x, -10, 0);
            dialogBubblePoint.localScale = new Vector3(1, 1, 1);
        }

        dialogBubble.transform.position = newPosition;
        dialogBubblePoint.transform.localPosition = new Vector3(_startingPositionDialogBubblePoint.x + targetPosition.x - newPosition.x, dialogBubblePoint.transform.localPosition.y, dialogBubblePoint.transform.localPosition.z);
    }
}
