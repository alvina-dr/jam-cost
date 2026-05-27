using PrimeTween;
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
    public List<ShopItem> BonusList = new();
    [SerializeField] private List<ShopItem> _boughtItemList = new();
    [SerializeField] private List<Transform> _boughtItemTransformList = new();
    [SerializeField] private GameObject _mask;

    public List<TextMeshProUGUI> _priceTextList = new();

    [SerializeField] private float _showBonusDelay;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _fallSparklesPS;

    private List<BonusData> _sellingBonusDataList = new();

    private void Start()
    {
        BonusMenu.SelectBonusList();
        _sellingBonusDataList = BonusDirector.Instance.GetRandomBonusRunList(3);

        for (int i = 0; i < BonusList.Count; i++)
        {
            BonusList[i].Setup(_sellingBonusDataList[i]);
        }

        for (int i = 0; i < _priceTextList.Count; i++)
        {
            if (i < BonusList.Count) _priceTextList[i].text = $"{BonusList[i].BonusData.Price}<sprite name=PP>";
            else _priceTextList[i].text = "...";
        }

        if (!SaveManager.CurrentSave.ShopFirstTime)
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("NPC1_ShopFirstTime");
            SaveManager.CurrentSave.ShopFirstTime = true;
        }
    }

    public void BuyShopItem(ShopItem shopItem)
    {
        _boughtItemList.Add(shopItem);
        shopItem.transform.position = _boughtItemTransformList[_boughtItemList.Count - 1].position;
        _fallSparklesPS.Play();
    }

    public void OpenVendingMachine()
    {
        VendingMachineAnimator.Play("Open");
        _mask.SetActive(false);

        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(_showBonusDelay);
        sequence.Group(Tween.ShakeLocalPosition(_vendingMachineSpriteRenderer.transform, Vector3.one * .3f, .3f));
        Sequence stretch = Sequence.Create();
        stretch.Group(Tween.ScaleX(_vendingMachineSpriteRenderer.transform, 1.5f, .2f));
        stretch.Group(Tween.ScaleX(_vendingMachineSpriteRenderer.transform, 1.11f, .3f));
        sequence.Group(stretch);
        for (int i = 0; i < _boughtItemList.Count; i++)
        {
            ShopItem shopItem = _boughtItemList[i];
            sequence.ChainCallback(() => shopItem.gameObject.SetActive(true));
            sequence.ChainCallback(() => shopItem.ShowBonus());
        }

        sequence.ChainDelay(.5f);

        for (int i = 0; i < _boughtItemList.Count; i++)
        {
            ShopItem shopItem = _boughtItemList[i];
            sequence.Chain(Tween.Scale(shopItem.transform, 1.1f, .15f));
            sequence.Chain(Tween.Scale(shopItem.transform, 1f, .1f));
            sequence.ChainCallback(() => shopItem.Collect());
            sequence.ChainDelay(.3f);
        }

        sequence.ChainDelay(1.5f);
        sequence.ChainCallback(() => LeaveShop());
    }

    public void CloseVendingMachine()
    {
        VendingMachineAnimator.Play("Close");
    }

    public void LeaveShop()
    {
        BonusMenu.ReleaseBonusList();
        SaveManager.Instance.NextNode();
    }

    //public void OpenVendingMachine()
    //{
    //    BonusMenu.OpenMenu();
    //}

    public void OpenConversionMachine()
    {
        ConversionMenu.OpenMenu();
    }
}
