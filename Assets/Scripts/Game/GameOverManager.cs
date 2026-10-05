using PrimeTween;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    #region Singleton
    public static GameOverManager Instance { get; private set; }

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

    [SerializeField] private Transform _mugMask;

    public void Open()
    {
        GameManager.Instance.UIManager.Timer.gameObject.SetActive(false);
        gameObject.SetActive(true);
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(_mugMask, 1, .5f));
        Tween.Position(_mugMask, Vector3.zero, .5f);
        sequence.ChainCallback(() => GameManager.Instance.UIManager.GameLost.OpenMenu());
    }
}
