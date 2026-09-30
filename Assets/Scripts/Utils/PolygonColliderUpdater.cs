using Sirenix.OdinInspector;
using UnityEngine;

public class PolygonColliderUpdater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    [Button]
    public void UpdatePolygonCollider()
    {
        if (TryGetComponent(out PolygonCollider2D polygon))
        {
            polygon.CreateFromSprite(_sprite.sprite);
        }
    }
}
