using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScoreCalculationManager : MonoBehaviour
{
    #region Singleton
    public static ScoreCalculationManager Instance { get; private set; }

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

    [SerializeField] private SortingGroup _sortingGroup;

    [Header("Items")]
    [SerializeField] private List<Item_ScoreCalculation> _itemList = new();
    [SerializeField] private Item_ScoreCalculation _itemPrefab;
    [SerializeField] private float _itemSpace;
    [SerializeField] private float _itemSize;
    [SerializeField] private Transform _itemParent;

    [Header("UI")]
    [SerializeField] private Button _continue;

    public void Show()
    {
        Setup();

        _sortingGroup.sortingLayerName = "ScoreCalculation";
        Tween.LocalPositionY(transform, 0, .5f);
        BonusHandManager.Instance.Show();

    }

    public void Setup()
    {
        List<ItemInstance> bagItemList = GameManager.Instance.ScavengingState.GetItemInstanceList();

        int size = GameManager.Instance.GetDepotSize();

        for (int i = 0; i < _itemList.Count; i++)
        {
            if (i < bagItemList.Count)
            {
                _itemList[i].Setup(bagItemList[i]);
            }
            else
            {
                _itemList[i].gameObject.SetActive(false);
            }
        }

        List<Item_ScoreCalculation> activeItemList = _itemList.FindAll(x => x.gameObject.activeSelf);
        for (int i = 0; i < activeItemList.Count; i++)
        {
            float totalSpace = _itemSpace * (activeItemList.Count - 1) + _itemSize * activeItemList.Count;
            activeItemList[i].transform.localPosition = new Vector3((i * _itemSpace) + (i * _itemSize + _itemSize / 2) - totalSpace / 2, 0);
        }
    }

    public void Hide()
    {
        BonusHandManager.Instance.Hide();
        Tween.LocalPositionY(transform, 10.8f, .5f).OnComplete(() => _sortingGroup.sortingLayerName = "Background");
    }

    public void AllowContinue()
    {
        _continue.gameObject.SetActive(true);
    }

    [Button]
    public void Instantiate(int number)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            DestroyImmediate(_itemList[i].gameObject);
        }

        _itemList.Clear();

        for (int i = 0; i < number; i++)
        {
            Item_ScoreCalculation bonusBehavior = PrefabUtility.InstantiatePrefab(_itemPrefab, _itemParent) as Item_ScoreCalculation;
            float totalSpace = _itemSpace * (number - 1) + _itemSize * number;
            bonusBehavior.transform.localPosition = new Vector3((i * _itemSpace) + (i * _itemSize + _itemSize / 2) - totalSpace / 2, 0);
            _itemList.Add(bonusBehavior);
        }
    }
}
