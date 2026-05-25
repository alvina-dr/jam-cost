using UnityEngine;
using UnityEngine.EventSystems;
using PrimeTween;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public BonusData BonusData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private float _fallSpeed;

    [SerializeField] private bool _boughtBonus;

    private Sequence _floatingSequence;

    public void Setup(BonusData bonusData)
    {
        BonusData = bonusData;
        _spriteRenderer.sprite = BonusData.Icon;
        if (_shadowSpriteRenderer) _shadowSpriteRenderer.sprite = BonusData.Icon;

        if (!_boughtBonus)
        {
            _floatingSequence = Sequence.Create(cycleMode: Sequence.SequenceCycleMode.Restart, cycles: -1);
            _floatingSequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, .05f, 1f, Ease.Linear));
            _floatingSequence.Chain(Tween.LocalPositionY(_spriteRenderer.transform, 0, 1f, Ease.Linear));
            _floatingSequence.isPaused = true;
            Tween.Delay(Random.Range(0, .5f), () => _floatingSequence.isPaused = false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_boughtBonus)
        {
            Sequence sequence = Sequence.Create();
            sequence.Chain(Tween.Scale(transform, 1.2f, .3f));
            sequence.Chain(Tween.Scale(transform, 0, .2f));
            sequence.ChainCallback(() => ShopManager.Instance.CloseVendingMachine());
            BonusData.GetBonus();
        }
        else
        {
            if (SaveManager.CurrentSave.CurrentRun.ProductivityPoints < BonusData.Price) return;

            SaveManager.Instance.AddPP(-BonusData.Price);
            SaveManager.CurrentSave.PPSpentRunShop += BonusData.Price;
            QuestDirector.Instance.CheckQuestCompletionByType<QD_PPSpentRunShop>();
            
            Sequence sequence = Sequence.Create();
            sequence.Chain(Tween.Scale(transform, 1.2f, .3f));
            sequence.Chain(Tween.Scale(transform, 1f, .2f));
            sequence.Chain(Tween.PositionAtSpeed(transform, new Vector3(transform.position.x, -3.5f, transform.position.z), _fallSpeed, Ease.Linear));
            sequence.ChainCallback(() => ShopManager.Instance.OpenVendingMachine(BonusData));
            sequence.ChainCallback(() => gameObject.SetActive(false));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(BonusData, transform.position);
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
        if (_floatingSequence.isAlive) _floatingSequence.isPaused = false;
    }
}
