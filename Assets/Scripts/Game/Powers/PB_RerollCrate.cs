using UnityEngine;

public class PB_RerollCrate : PowerBehavior
{
    public void Start()
    {
        GameManager.Instance.ItemManager.ResetDumpster();
        Destroy(gameObject);
    }
}
