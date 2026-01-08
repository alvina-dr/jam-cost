using UnityEngine;

public class HubManagerRelay : MonoBehaviour
{
    public void OpenLocker() => HubManager.Instance.OpenLocker();
    public void CloseLocker() => HubManager.Instance.CloseLocker();
    public void LaunchNewRun() => HubManager.Instance.LaunchNewRun();
}
