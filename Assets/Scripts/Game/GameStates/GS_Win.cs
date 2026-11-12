using System.Collections.Generic;
using UnityEngine;

public class GS_Win : GameState
{
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private List<ParticleSystem> _particleSystemList = new();

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 1f;
        GameManager.Instance.SetGameState(GameManager.Instance.RewardState);

        for (int i = 0; i < _particleSystemList.Count; i++)
        {
            _particleSystemList[i].Play();
        }
        //GameManager.Instance.UIManager.GameWon.OpenMenu();
        // show small victory animation
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        //GameManager.Instance.UIManager.GameWon.CloseMenu();
    }
}
