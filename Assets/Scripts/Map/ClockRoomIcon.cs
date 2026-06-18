using UnityEngine;

public class ClockRoomIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _unlockedSprite;

    public void Enable()
    {
        _unlockedSprite.gameObject.SetActive(true);
    }

    public void Disable()
    {
        _unlockedSprite.gameObject.SetActive(false);
    }
}
