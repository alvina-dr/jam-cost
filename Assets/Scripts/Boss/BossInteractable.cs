using EasyTransition;
using UnityEngine;

public class BossInteractable : MonoBehaviour
{
    [SerializeField] private TransitionSettings _transitionSettings;
    [SerializeField] private MapNodeData _bossMapNode;

    public void StartBossGame()
    {
        SaveManager.Instance.CurrentMapNode = _bossMapNode;
        TransitionManager.Instance().TransitionChangeScene("Game", _transitionSettings, 0);
    }
}
