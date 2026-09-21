using PrimeTween;
using Sirenix.OdinInspector;
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
    [SerializeField] private BonusBehavior _bonusBehaviorPrefab;
    [SerializeField] private float _bonusSpace;
    [SerializeField] private float _bonusSize;

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
            }
            else
            {
                _bonusBehaviorList[i].gameObject.SetActive(false);
            }
        }

        List<BonusBehavior> activeBonusList =  _bonusBehaviorList.FindAll(x => x.gameObject.activeSelf);
        for (int i = 0; i < activeBonusList.Count; i++)
        {
            float totalSpace = _bonusSpace * (activeBonusList.Count - 1) + _bonusSize * activeBonusList.Count;
            activeBonusList[i].transform.localPosition = new Vector3((i * _bonusSpace) + (i * _bonusSize + _bonusSize / 2) - totalSpace / 2, -3);
        }
    }

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
            float totalSpace = _bonusSpace * (number - 1) + _bonusSize * number;
            bonusBehavior.transform.localPosition = new Vector3((i * _bonusSpace) + (i * _bonusSize + _bonusSize / 2) - totalSpace / 2, 0);
            _bonusBehaviorList.Add(bonusBehavior);
        }
    }

    [Button]
    public void Show()
    {
        if (IsShow) return;

        IsShow = true;
        List<BonusBehavior> activeBonusList = _bonusBehaviorList.FindAll(x => x.gameObject.activeSelf);
        for (int i = 0; i < activeBonusList.Count; i++)
        {
            int index = i;
            Sequence bonusSequence = Sequence.Create();
            bonusSequence.ChainDelay(index * .1f + 0.1f);
            bonusSequence.Chain(Tween.LocalPositionY(activeBonusList[index].transform, 0.3f, .1f));
            bonusSequence.Chain(Tween.LocalPositionY(activeBonusList[index].transform, 0, .2f));
        }
    }

    [Button]
    public void Hide()
    {
        if (!IsShow) return;

        IsShow = false;
        List<BonusBehavior> activeBonusList = _bonusBehaviorList.FindAll(x => x.gameObject.activeSelf);
        for (int i = 0; i < activeBonusList.Count; i++)
        {
            int index = i;
            Sequence bonusSequence = Sequence.Create();
            bonusSequence.ChainDelay(index * .1f + 0.1f);
            bonusSequence.Chain(Tween.LocalPositionY(activeBonusList[index].transform, 0.3f, .1f));
            bonusSequence.Chain(Tween.LocalPositionY(activeBonusList[index].transform, -3, .2f));
        }
    }
}
