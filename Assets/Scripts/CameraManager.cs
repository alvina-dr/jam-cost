using MoreMountains.Feedbacks;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    public static CameraManager Instance { get; private set; }

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

    public MMWiggle Shake;

    public void SimpleShake()
    {
        Shake.WigglePosition(.2f);
    }
}
