using UnityEngine;

[CreateAssetMenu(fileName = "ITD_Hesitant", menuName = "Scriptable Objects/ItemTagData/ITD_Hesitant")]
public class ITD_Hesitant : ItemTagData
{
    [SerializeField] private Vector2Int _range;

    public override float CalculateMult(ItemData data)
    {
        return Random.Range(_range.x, _range.y);
    }
}
