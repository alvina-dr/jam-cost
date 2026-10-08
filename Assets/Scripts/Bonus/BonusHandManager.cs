using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BonusHandManager : MonoBehaviour
{
    #region Singleton
    public static BonusHandManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] private List<BonusBehavior> _bonusBehaviorList = new();
    [SerializeField] private List<BonusBehavior> _activeBonusBehaviorList = new();
    [SerializeField] private BonusBehavior _bonusBehaviorPrefab;
    [SerializeField] private float _bonusSpace;
    [SerializeField] private float _bonusSize;

    [SerializeField] private float _animationSpeedMultiplier;

    public bool IsShow;

    private void Start()
    {
        SetupBonusHand();
    }

    public void SetupBonusHand()
    {
        for (int i = 0; i < _bonusBehaviorList.Count; i++)
        {
            if (i < SaveManager.Instance.CurrentRunBonusList.Count)
            {
                _bonusBehaviorList[i].Setup(SaveManager.Instance.CurrentRunBonusList[i]);
                _activeBonusBehaviorList.Add(_bonusBehaviorList[i]);
            }
            else
            {
                _bonusBehaviorList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _activeBonusBehaviorList.Count; i++)
        {
            float totalSpace = _bonusSpace * (_activeBonusBehaviorList.Count - 1) + _bonusSize * _activeBonusBehaviorList.Count;
            _activeBonusBehaviorList[i].transform.localPosition = new Vector3((i * _bonusSpace) + (i * _bonusSize + _bonusSize / 2) - totalSpace / 2, -3);
        }
    }

    public BonusBehavior GetBonus(string name)
    {
        return _bonusBehaviorList.Find(x => x.BonusData.Name == name);
    }

    public void AddBonus(BonusData bonusData)
    {
        if (_activeBonusBehaviorList.Count >= _bonusBehaviorList.Count ) return;

        int futureBonusCount = _activeBonusBehaviorList.Count + 1;
        float totalSpace = _bonusSpace * (futureBonusCount - 1) + _bonusSize * futureBonusCount;
        for (int i = 0; i < _activeBonusBehaviorList.Count; i++)
        {
            int index = i;
            float normalPositionX = (i * _bonusSpace) + (i * _bonusSize + _bonusSize / 2) - totalSpace / 2;

            Sequence moveSequence = Sequence.Create();
            moveSequence.ChainDelay(index * .05f + 0.1f);
            moveSequence.Chain(Tween.LocalPositionX(_activeBonusBehaviorList[index].transform, normalPositionX, .1f));
        }

        BonusBehavior bonusBehavior = _bonusBehaviorList[_activeBonusBehaviorList.Count];
        _activeBonusBehaviorList.Add(bonusBehavior);
        bonusBehavior.Setup(bonusData);
        bonusBehavior.transform.localPosition = new Vector3(((futureBonusCount - 1) * _bonusSpace) + ((futureBonusCount - 1) * _bonusSize + _bonusSize / 2) - totalSpace / 2, -3);
        Sequence addBonusSequence = Sequence.Create();
        addBonusSequence.ChainDelay(futureBonusCount * .05f + 0.1f);
        addBonusSequence.ChainCallback(() => AudioManager.PlaySound(AudioManager.Instance.BonusShow));
        addBonusSequence.Chain(Tween.LocalPositionY(bonusBehavior.transform, 0.3f, .1f));
        addBonusSequence.Chain(Tween.LocalPositionY(bonusBehavior.transform, 0, .2f));
    }

    [Button]
    public void Show()
    {
        if (IsShow) return;

        IsShow = true;
        float totalDelay = 0.1f;
        for (int i = 0; i < _activeBonusBehaviorList.Count; i++)
        {
            int index = i;
            totalDelay += 1.0f / (index + 2.0f) * _animationSpeedMultiplier;
            float delay = totalDelay;
            Sequence bonusSequence = Sequence.Create();
            bonusSequence.ChainDelay(delay);
            bonusSequence.ChainCallback(() => AudioManager.PlaySound(AudioManager.Instance.BonusShow, index  * AudioManager.Instance.BonusShow.PitchBonus));
            bonusSequence.Chain(Tween.LocalPositionY(_activeBonusBehaviorList[index].transform, 0.3f, .1f));
            bonusSequence.Chain(Tween.LocalPositionY(_activeBonusBehaviorList[index].transform, 0, .2f));
        }
    }

    [Button]
    public void Hide()
    {
        if (!IsShow) return;

        IsShow = false;
        List<BonusBehavior> activeBonusList = _bonusBehaviorList.FindAll(x => x.gameObject.activeSelf);
        float totalDelay = 0.1f;
        for (int i = 0; i < activeBonusList.Count; i++)
        {
            int index = i;
            totalDelay += 1.0f / (index + 2.0f) * _animationSpeedMultiplier;
            float delay = totalDelay;
            Sequence bonusSequence = Sequence.Create();
            bonusSequence.ChainDelay(delay);
            bonusSequence.ChainCallback(() => AudioManager.PlaySound(AudioManager.Instance.BonusHide, index * -AudioManager.Instance.BonusHide.PitchBonus));
            bonusSequence.Chain(Tween.LocalPositionY(activeBonusList[index].transform, 0.3f, .1f));
            bonusSequence.Chain(Tween.LocalPositionY(activeBonusList[index].transform, -3, .2f));
        }
    }

#if UNITY_EDITOR
    [Button]
    public void Instantiate(int number)
    {
        for (int i = 0; i < _bonusBehaviorList.Count; i++)
        {
            DestroyImmediate(_bonusBehaviorList[i].gameObject);
        }

        _bonusBehaviorList.Clear();

        for (int i = 0; i < number; i++)
        {
            BonusBehavior bonusBehavior = PrefabUtility.InstantiatePrefab(_bonusBehaviorPrefab, transform) as BonusBehavior;
            bonusBehavior.name = "BonusBehavior_" + i;
            float totalSpace = _bonusSpace * (number - 1) + _bonusSize * number;
            bonusBehavior.transform.localPosition = new Vector3((i * _bonusSpace) + (i * _bonusSize + _bonusSize / 2) - totalSpace / 2, 0);
            _bonusBehaviorList.Add(bonusBehavior);
        }
    }

#endif
}
