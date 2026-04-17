using UnityEngine;
using UnityEngine.EventSystems;

public class DoorAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Open")) return;

        float currentTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currentTime < 1)
        {
            _animator.Play("Open", 0, 1 - _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else
        {
            _animator.Play("Open");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Close")) return;

        float currentTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currentTime < 1)
        {
            _animator.Play("Close", 0, 1 - _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else
        {
            _animator.Play("Close");
        }
    }
}
