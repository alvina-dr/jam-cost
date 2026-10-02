using System;
using System.Collections;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private AnimationData _animation;
    [SerializeField] private float _delayBetweenSprites;
    [SerializeField] private bool _startOnAwake;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _disabledOnStart;
    [SerializeField] private float _delayBetweenLoop;
    private Coroutine _routine;

    private void Awake()
    {
        if (_disabledOnStart) _sprite.enabled = false;
        if (_startOnAwake) _routine = StartCoroutine(Play());
    }

    private void OnDisable()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
        }
    }

    public void StartAnim(Action callback = null)
    {
        StopAnim();

        if (gameObject.activeSelf)
        {
            _routine = StartCoroutine(Play(callback));
        }
    }

    public void StartAnim()
    {
        StartAnim(null);
    }

    public void StopAnim()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
        }

        if (_animation) _sprite.sprite = _animation.SpriteList[0];
    }

    public IEnumerator Play(Action callback = null)
    {
        _sprite.enabled = true;
        for (int i = 0; i < _animation.SpriteList.Count; i++)
        {
            _sprite.sprite = _animation.SpriteList[i];
            yield return new WaitForSecondsRealtime(_animation.DelayBetweenSprites);
        }
        //_image.enabled = false;

        if (callback != null) callback?.Invoke();

        yield return new WaitForSecondsRealtime(_delayBetweenLoop);
        _routine = null;
        if (_loop) _routine = StartCoroutine(Play());
    }

    public void SetAnimation(AnimationData animation)
    {
        _animation = animation;
    }

    public void SetLoop(bool loop)
    {
        _loop = loop;
    }
}
