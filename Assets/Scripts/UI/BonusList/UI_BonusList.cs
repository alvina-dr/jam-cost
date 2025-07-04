using UnityEngine;

public class UI_BonusList : MonoBehaviour
{
    [SerializeField] private UI_BonusIcon _bonusIconPrefab;
    [SerializeField] private Transform _bonusIconParent;

    public void UpdateBonusList()
    {
        for (int i = 0; i < GameManager.Instance.BonusList.Count; i++)
        {
            UI_BonusIcon bonusIcon = Instantiate(_bonusIconPrefab, _bonusIconParent);
            bonusIcon.Setup(GameManager.Instance.BonusList[i]);
        }
    }
}
