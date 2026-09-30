using UnityEngine;

public class Interactable_Lever : Interactable
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SimpleAnimation _animation;
    
    public void OnMouseDown()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

        _collider.enabled = false;
        _animation.StartAnim(() =>
        {
            _collider.enabled = true;
        });
        GameManager.Instance.ApproveDepot();
    }
}
