using UnityEngine;

public class Interactable_Lever : Interactable
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SimpleAnimation _animation;
    
    public void OnMouseDown()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;
        if (GameManager.Instance.ScavengingState.SelectedItemList.Count == 0) return;

        AudioManager.PlaySound(AudioManager.Instance.Combination);
        _collider.enabled = false;
        _animation.StartAnim(() =>
        {
            _collider.enabled = true;
        });
        GameManager.Instance.ApproveDepot();
    }
}
