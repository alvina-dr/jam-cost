using DG.Tweening;
using System;
using UnityEngine;

public class UI_ShowSizeAnimation : MonoBehaviour
{
    [SerializeField] private float _showSize;
    [SerializeField] private bool _hideOnStart;
    public bool IsVisible = false;

    private void Start()
    {
        if (_hideOnStart)
        {
            Hide(isAnimated:false);
        }
    }

    public void Show(Action callback = null)
    {
        IsVisible = true;
        transform.DOKill();

        Sequence sizeUp = DOTween.Sequence()
            .SetLink(gameObject)
            .SetUpdate(true);

        sizeUp.Append(transform.DOScale(_showSize + (_showSize * .1f), .2f));
        sizeUp.Append(transform.DOScale(_showSize, .1f));
        sizeUp.AppendCallback(() => callback?.Invoke());
    }

    public void Hide(Action callback = null, bool isAnimated = true)
    {
        IsVisible = false;
        transform.DOKill();

        if (isAnimated)
        {
            Sequence sizeDown = DOTween.Sequence()
                .SetLink(gameObject)
                .SetUpdate(true);

            sizeDown.Append(transform.DOScale(_showSize + (_showSize * .1f), .2f));
            sizeDown.Append(transform.DOScale(0, .1f));
            sizeDown.AppendCallback(() => callback?.Invoke());
        }
        else
        {
            transform.localScale = Vector3.zero;
            callback?.Invoke();
        }

    }
}
