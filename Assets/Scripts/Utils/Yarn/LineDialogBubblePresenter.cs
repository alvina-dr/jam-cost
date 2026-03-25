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

    public Transform dialogBubble;

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
                Typewriter = ValidateCustomTypewriter();
                Typewriter?.ActionMarkupHandlers.AddRange(ActionMarkupHandlers);
                if (Typewriter == null)
                {
                    Debug.LogWarning("Typewriter mode is set to custom but there is no typewriter set.");
                }
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
            dialogBubble.transform.position = Camera.main.WorldToScreenPoint(dialogBubbleCharacter.transform.position);
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
            _sequence = Sequence.Create();
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
            _sequence = Sequence.Create();
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
}
