using UnityEngine;

public class ShopManagerRelay : MonoBehaviour
{
    public void LeaveShop() => ShopManager.Instance.LeaveShop();
    public void OpenVendingMachine() => ShopManager.Instance.OpenVendingMachine();
    public void OpenConvertingMachine() => ShopManager.Instance.OpenConvertingMachine();
}
