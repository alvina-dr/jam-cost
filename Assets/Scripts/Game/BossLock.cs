using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class BossLock : MonoBehaviour
{
    public List<ItemData> itemDataList = new();
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _textInfo;

    public bool IsLocked => itemDataList.Count > 0; 

    public void SetLock()
    {
        gameObject.SetActive(true);
        // find 3 random items in item list matching criterias
        List<ItemBehavior> selectableList = GameManager.Instance.ItemManager.ItemList.FindAll(x => x is DraggableBehavior);
        selectableList = selectableList.FindAll(x => x.Item.Data.Price > 0);

        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, selectableList.Count);
            itemDataList.Add(selectableList[random].Item.Data);
            selectableList.RemoveAt(random);
        }
        UpdateLockVisual();
    }

    public bool TryOpenLock(ItemData itemData)
    {
        if (itemData.Save.Name == itemDataList[0].Save.Name)
        {
            OpenLock();
            if (BossBehavior_Possession.Instance != null)
            {
                BossBehavior_Possession.Instance.UnlockDepositBox();
            }
            return true;
        }
        else return false;
    }

    public void OpenLock()
    {
        itemDataList.RemoveAt(0);

        if (itemDataList.Count == 0 )
        {
            DestroyLock();
        }
        else
        {
            UpdateLockVisual();
        }
    }

    public void DestroyLock()
    {
        gameObject.SetActive(false);
    }

    public void UpdateLockVisual()
    {
        _textInfo.text = $"{itemDataList.Count}/3";
        _spriteRenderer.sprite = itemDataList[0].Icon;
    }
}
