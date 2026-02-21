using UnityEngine;

public class PB_MoreTime : PowerBehavior
{
    private void Start()
    {
        GameManager.Instance.ScavengingState.Timer += 10;
        Destroy(gameObject);
    }
}
