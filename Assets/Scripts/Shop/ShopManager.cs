using DG.Tweening;
using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager Instance { get; private set; }

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

    [Header("References")]
    public UI_BonusMenu BonusMenu;
    public UI_ConversionMenu ConversionMenu;

    [Header("Vending Machine")]
    public Animator VendingMachineAnimator;
    [SerializeField] private SpriteRenderer _vendingMachineSpriteRenderer;
    public List<ShopParentChoice> BonusList = new();
    [SerializeField] private List<ShopParentChoice> _boughtItemList = new();
    [SerializeField] private GameObject _mask;

    public List<TextMeshProUGUI> _priceTextList = new();

    [SerializeField] private float _showBonusDelay;
    [SerializeField] private UI_TextValue _rerollButtonText;
    [SerializeField] private UI_Button _rerollButton;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _fallSparklesPS;

    private List<BonusData> _sellingBonusDataList = new();

    private void Start()
    {
        SetNewRandomBonus();

        for (int i = 0; i < BonusList.Count; i++)
        {
            BonusList[i].MatchingPrice = _priceTextList[i];
        }

        if (!SaveManager.CurrentSave.ShopFirstTime)
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("NPC1_ShopFirstTime");
            SaveManager.CurrentSave.ShopFirstTime = true;
        }

        UpdateRerollButton();

        BonusHandManager.Instance.Show();
    }

    public void SetNewRandomBonus()
    {
        _sellingBonusDataList = BonusDirector.Instance.GetRandomBonusRunList(3, true);

        for (int i = 0; i < BonusList.Count; i++)
        {
            BonusList[i].Item.Setup(_sellingBonusDataList[i]);
        }

        for (int i = 0; i < _priceTextList.Count; i++)
        {
            if (i < BonusList.Count) _priceTextList[i].text = $"{BonusList[i].Item.BonusData.Price}<sprite name=PP>";
            else _priceTextList[i].text = "...";
        }
    }

    public void BuyShopItem(ShopParentChoice shopParentChoice)
    {
        _boughtItemList.Add(shopParentChoice);
        //shopItem.transform.position = _boughtItemTransformList[_boughtItemList.Count - 1].position;
        _fallSparklesPS.Play();
    }

    public void LeaveShop()
    {
        BonusMenu.ReleaseBonusList();
        SaveManager.Instance.NextNode();
    }

    public void UpdateRerollButton()
    {
        _rerollButtonText.SetTextValue($"Reroll ({SaveManager.CurrentSave.CurrentRun.Rerolls})", false);
        if (SaveManager.CurrentSave.CurrentRun.Rerolls <= 0)
        {
            _rerollButtonText.SetTextColor(Color.grey);
            _rerollButton.transform.DOScale(1f, .3f).SetUpdate(true);
            _rerollButton.enabled = false;
        }
        else
        {
            _rerollButtonText.SetTextColor(new Color32(231, 93, 90, 255));
            _rerollButton.enabled = true;
        }
    }

    public void RerollShop()
    {
        if (SaveManager.CurrentSave.CurrentRun.Rerolls == 0) return;

        SaveManager.CurrentSave.CurrentRun.Rerolls--;
        UpdateRerollButton();
        BonusMenu.ReleaseBonusList();
        SetNewRandomBonus();
    }

    [Button]
    public void DebugReroll()
    {
        BonusMenu.ReleaseBonusList();
        SetNewRandomBonus();
    }
}
