using System.Collections.Generic;
using UnityEngine;

public class UI_BonusList : MonoBehaviour
{
    [SerializeField] private UI_BonusIcon _bonusIconPrefab;
    [SerializeField] private Transform _bonusIconParent;
    [SerializeField] private List<UI_BonusIcon> _bonusIconList = new();

    public Transform GetBonusTextSpawnPoint(BonusData bonusData)
    {
        UI_BonusIcon bonusIcon = _bonusIconList.Find(x => x.Data == bonusData);

        if (bonusIcon != null) return bonusIcon.InfoSpawnPoint;
        else return null;
    }

    public void UpdateBonusList()
    {
        for (int i = _bonusIconList.Count - 1; i >= 0; i--)
        {
            Destroy(_bonusIconList[i].gameObject);
        }

        _bonusIconList.Clear();

        for (int i = 0; i < GameManager.Instance.BonusList.Count; i++)
        {
            UI_BonusIcon bonusIcon = Instantiate(_bonusIconPrefab, _bonusIconParent);
            bonusIcon.Setup(GameManager.Instance.BonusList[i]);
            _bonusIconList.Add(bonusIcon);
        }
    }
}
