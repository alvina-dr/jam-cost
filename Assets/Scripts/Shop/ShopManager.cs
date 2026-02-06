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
    public UIManager UIManager;

    public void LeaveShop()
    {
        SaveManager.Instance.NextDay();
    }

    public void OpenVendingMachine()
    {
        UIManager.BonusMenu.OpenMenu();
    }

    public void OpenConvertingMachine()
    {

    }
}
