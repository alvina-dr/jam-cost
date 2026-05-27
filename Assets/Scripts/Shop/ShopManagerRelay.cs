using UnityEngine;

public class ShopManagerRelay : MonoBehaviour
{
    public void LeaveShop() => ShopManager.Instance.LeaveShop();
    public void OpenVendingMachine() => ShopManager.Instance.OpenVendingMachine();
    public void OpenConversionMachine() => ShopManager.Instance.OpenConversionMachine();
}
