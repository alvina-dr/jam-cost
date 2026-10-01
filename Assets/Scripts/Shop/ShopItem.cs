using UnityEngine;
using UnityEngine.EventSystems;
using PrimeTween;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public BonusData BonusData;
    [SerializeField] private ShopParentChoice _parentChoice;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private float _fallSpeed;

    private Sequence _floatingSequence;

    public void Setup(BonusData bonusData)
    {
        BonusData = bonusData;
        _spriteRenderer.sprite = BonusData.Icon;
        if (_shadowSpriteRenderer) _shadowSpriteRenderer.sprite = BonusData.Icon;

        _floatingSequence = Sequence.Create(cycleMode: Sequence.SequenceCycleMode.Restart, cycles: -1);
        _floatingSequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, .05f, 1f, Ease.Linear));
        _floatingSequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, 0, 1f, Ease.Linear));
        _floatingSequence.isPaused = true;
        Tween.Delay(Random.Range(0, .5f), () => _floatingSequence.isPaused = false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SaveManager.CurrentSave.CurrentRun.ProductivityPoints < BonusData.Price) return;

        SaveManager.Instance.AddPP(-BonusData.Price);
        SaveManager.CurrentSave.PPSpentRunShop += BonusData.Price;
        QuestDirector.Instance.CheckQuestCompletionByType<QD_PPSpentRunShop>();
        Collect();
        ShopManager.Instance.BuyShopItem(this);
        Tween.Delay(.5f, () => BonusHandManager.Instance.AddBonus(BonusData));
    }

    public void Collect()
    {
        _floatingSequence.Stop();
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(transform, 1.2f, .3f));
        sequence.Chain(Tween.Scale(transform, 0, .2f));
        BonusData.GetBonus();
        _parentChoice.Face.BuyBonus();
    }

    public void ShowBonus()
    {
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(transform, 1.2f, .15f));
        sequence.Chain(Tween.Scale(transform, 1, .1f));
    }

    private void OnMouseEnter()
    {
        TooltipManager.Instance.ShowTooltip(BonusData, transform.position, Vector3.up * 60);
        _parentChoice.Face.MouseEnterFace();
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = true;
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
        _parentChoice.Face.MouseExitFace();
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(BonusData, transform.position, Vector3.up * 60);
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = false;
    }
}
