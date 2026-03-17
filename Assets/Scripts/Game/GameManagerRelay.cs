using UnityEngine;

public class GameManagerRelay : MonoBehaviour
{
    public void RerollCrate() => GameManager.Instance.RerollCrate();
}
