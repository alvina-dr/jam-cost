using PrimeTween;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StickerManager : MonoBehaviour
{
    #region Singleton
    public static StickerManager Instance { get; private set; }

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

    public OverCheck SelectCardZone;
    public GameObject StickerPlacement;
    public StickerBehavior StickerBehavior;
    public Transform BonusPosition;
    public Transform StickerPosition;

    public StickerData CurrentStickerData;
    public BonusBehavior CurrentBonusBehavior;

    private void Start()
    {
        Tween.Delay(.3f, () => BonusHandManager.Instance.Show());
        CurrentStickerData = StickerDirector.GetRandomSticker();
        StickerBehavior.Setup(CurrentStickerData);
    }

    public void SelectCard(BonusBehavior bonusBehavior)
    {
        CurrentBonusBehavior = bonusBehavior;
        bonusBehavior.BehaviorAddSticker.Setup();
        bonusBehavior.enabled = false;

        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(bonusBehavior.transform, 2.3f, .3f));
        sequence.Chain(Tween.Scale(bonusBehavior.transform, 2.0f, .2f));
        sequence.Chain(Tween.Position(bonusBehavior.transform, BonusPosition.position, .5f));

        StickerPlacement.gameObject.SetActive(true);

        StickerBehavior.StickMode();
    }
}
