using UnityEngine;

public class ShopManagerRelay : MonoBehaviour
{
    public void LeaveShop() => ShopManager.Instance.LeaveShop();
}
