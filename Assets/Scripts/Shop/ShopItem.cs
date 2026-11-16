using UnityEngine;
using PrimeTween;

public class ShopItem : MonoBehaviour
{
    public BonusInstance BonusInstance;
    [SerializeField] private ShopParentChoice _parentChoice;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private float _fallSpeed;

    private Sequence _floatingSequence;

    public void Setup(BonusInstance bonusInstance)
    {
        BonusInstance = bonusInstance;
        _spriteRenderer.sprite = BonusInstance.Data.Icon;
        if (_shadowSpriteRenderer) _shadowSpriteRenderer.sprite = BonusInstance.Data.Icon;

        _floatingSequence = Sequence.Create(cycleMode: Sequence.SequenceCycleMode.Restart, cycles: -1);
        _floatingSequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, .05f, 1f, Ease.Linear));
        _floatingSequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, 0, 1f, Ease.Linear));
        _floatingSequence.isPaused = true;
        Tween.Delay(Random.Range(0, .5f), () => _floatingSequence.isPaused = false);
    }

    private void OnMouseDown()
    {
        if (SaveManager.CurrentSave.CurrentRun.ProductivityPoints < BonusInstance.Data.Price) return;

        SaveManager.Instance.AddPP(-BonusInstance.Data.Price);
        SaveManager.CurrentSave.PPSpentRunShop += BonusInstance.Data.Price;
        QuestDirector.Instance.CheckQuestCompletionByType<QD_PPSpentRunShop>();
        ShopManager.Instance.BuyShopItem(_parentChoice);
        _parentChoice.MatchingPrice.gameObject.SetActive(false);
        Collect();
    }

    public void Collect()
    {
        _floatingSequence.Stop();
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(transform, 1.2f, .3f));
        sequence.Chain(Tween.Scale(transform, 0, .2f));
        BonusInstance.GetBonus();
        _parentChoice.Face.BuyBonus();
        Tween.Delay(.5f, () => BonusHandManager.Instance.AddBonus(BonusInstance));
    }

    public void ShowBonus()
    {
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(transform, 1.2f, .15f));
        sequence.Chain(Tween.Scale(transform, 1, .1f));
    }

    private void OnMouseEnter()
    {
        TooltipManager.Instance.ShowTooltip(BonusInstance, transform.position, Vector3.up * 60);
        _parentChoice.Face.MouseEnterFace();
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = true;
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
        _parentChoice.Face.MouseExitFace();
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = false;
    }
}
