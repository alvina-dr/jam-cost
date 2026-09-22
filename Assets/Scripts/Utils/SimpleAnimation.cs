using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private List<Sprite> _spriteList = new();
    [SerializeField] private float _delayBetweenSprites;
    [SerializeField] private bool _startOnAwake;
    [SerializeField] private bool _loop;
    [SerializeField] private float _delayBetweenLoop;
    private Coroutine _routine;

    private void Awake()
    {
        _sprite.enabled = false;
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
        if (gameObject.activeSelf)
        {
            _routine = StartCoroutine(Play(callback));
        }
    }

    public IEnumerator Play(Action callback = null)
    {
        _sprite.enabled = true;
        for (int i = 0; i < _spriteList.Count; i++)
        {
            _sprite.sprite = _spriteList[i];
            yield return new WaitForSecondsRealtime(_delayBetweenSprites);
        }
        //_image.enabled = false;

        if (callback != null) callback?.Invoke();

        yield return new WaitForSecondsRealtime(_delayBetweenLoop);
        _routine = null;
        if (_loop) _routine = StartCoroutine(Play());
    }

    public void SetAnimation(List<Sprite> spriteList)
    {
        _spriteList = spriteList;
    }
}
