using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private List<Animation> _openAnimationList = new();
    [SerializeField] private List<Animation> _closeAnimationList = new();
    private bool _isOpen = false;
    public bool IsOpen() => _isOpen;

    public void OpenMenu()
    {
        _isOpen = true;
        gameObject.SetActive(true);
        float totalDelay = 0;
        for (int i = 0; i < _openAnimationList.Count; i++)
        {
            int index = i;
            totalDelay += _openAnimationList[index].Delay + _openAnimationList[index].Time;
            DOVirtual.DelayedCall(totalDelay, () =>
            {
                Pop(_openAnimationList[index].Transform, _openAnimationList[index].Time, true);
            }).SetUpdate(true);
        }
    }

    public void CloseMenu()
    {
        _isOpen = false;
        gameObject.SetActive(false);
        float totalDelay = 0;
        for (int i = 0; i < _closeAnimationList.Count; i++)
        {
            int index = i;
            totalDelay += _closeAnimationList[index].Delay + _closeAnimationList[index].Time;
            DOVirtual.DelayedCall(totalDelay, () =>
            {
                Pop(_closeAnimationList[index].Transform, _closeAnimationList[index].Time, false);
            }).SetUpdate(true);
        }
    }

    public void Pop(Transform transform, float time, bool show)
    {
        if (show) transform.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.SetUpdate(true);
        animationSequence.Append(transform.DOScale(1.3f, time/2));

        if (show) animationSequence.Append(transform.DOScale(1f, time / 2));
        else animationSequence.Append(transform.DOScale(0f, time/2));
    }

    [Serializable]
    public class Animation
    {
        public Transform Transform;
        public float Time;
        public float Delay;
    }
}
