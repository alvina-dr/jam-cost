using Sirenix.OdinInspector;
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

    public void LeaveShop()
    {
        SaveManager.Instance.NextDay();
    }

    public void OpenVendingMachine()
    {
        BonusMenu.OpenMenu();
    }

    public void OpenConversionMachine()
    {
        ConversionMenu.OpenMenu();
    }
}
