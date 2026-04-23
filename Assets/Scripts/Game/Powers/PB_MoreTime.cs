using UnityEngine;
using PrimeTween; 

public class PB_MoreTime : PowerBehavior
{
    private void Start()
    {
        Sequence sequence = Sequence.Create();

        for (int i = 0; i < 10; i++)
        {
            sequence.ChainCallback(() => GameManager.Instance.UIManager.OnParticleAttracted_Time(1));
            sequence.ChainCallback(() => GameManager.Instance.UIManager.AddTimerParticle(1, transform.position));
            sequence.ChainDelay(.05f);
        }
        sequence.ChainCallback(() =>
        {
            Destroy(gameObject);
        });
    }
}
