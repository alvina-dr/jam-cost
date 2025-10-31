using DG.Tweening;
using UnityEngine;

public class UI_MovingBackground : MonoBehaviour
{
    [SerializeField] private float _time;

    private void Start()
    {
        transform.DOLocalMoveX(transform.localPosition.x + 1920, _time).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
