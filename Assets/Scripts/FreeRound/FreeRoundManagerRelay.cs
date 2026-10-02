using UnityEngine;

public class FreeRoundManagerRelay : MonoBehaviour
{
    public void LeaveFreeRound() => FreeRoundManager.Instance.LeaveFreeRound();
    public void GetTimeBonus() => FreeRoundManager.Instance.GetTimeBonus();
}
