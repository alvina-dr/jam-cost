using UnityEngine;

public class FreeRoundManagerRelay : MonoBehaviour
{
    public void LeaveFreeRound() => FreeRoundManager.Instance.LeaveFreeRound();
    public void GetFreeRound() => FreeRoundManager.Instance.GetFreeRound();
}
