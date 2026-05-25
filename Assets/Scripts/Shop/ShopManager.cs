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

    public Animator VendingMachineAnimator;
    public List<ShopItem> BonusList = new();
    public ShopItem BoughtItem;

    public List<TextMeshProUGUI> _priceTextList = new();

    [SerializeField] private float _showBonusDelay;

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

    public void OpenVendingMachine(BonusData bonusData)
    {
        VendingMachineAnimator.Play("Open");
        BoughtItem.Setup(bonusData);
        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(_showBonusDelay);
        sequence.Chain(Tween.Scale(BoughtItem.transform, 1.1f, .3f));
        sequence.Chain(Tween.Scale(BoughtItem.transform, 1f, .2f));
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
