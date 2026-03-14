using UnityEngine;
using PrimeTween; 

public class PB_MoreTime : PowerBehavior
{
    private void Start()
    {
        Sequence sequence = Sequence.Create();

        for (int i = 0; i < 10; i++)
        {
            sequence.ChainCallback(() => GameManager.Instance.UIManager.AddTimer(1, Vector3.zero));
            sequence.ChainDelay(.05f);
        }
        sequence.ChainCallback(() =>
        {
            Destroy(gameObject);
        });
    }
}
