using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonusInstance
{
    public BonusData Data;
    public List<StickerInstance> StickerList = new();

    public void AddSticker(StickerBehavior stickerBehavior, SpriteRenderer stickerSprite)
    {
        StickerList.Add(new StickerInstance(stickerBehavior.StickerData, stickerSprite.transform.localPosition));
    }

    public virtual void GetBonus()
    {
        switch (Data.Durability)
        {
            case BonusDurability.Run:
                SaveManager.Instance.CurrentRunBonusList.Add(this);
                break;
            case BonusDurability.Permanent:
                //SaveManager.Instance.PermanentBonusList.Add(this);
                break;
        }
    }

    public BonusInstance(BonusData data)
    {
        Data = data;
    }

    public BonusInstance(BonusInstanceSave bonusInstanceSave)
    {
        Data = BonusDirector.Instance.RunBonusDataDictionary[bonusInstanceSave.BonusDataName];
        for (int i = 0; i < bonusInstanceSave.StickerList.Count; i++)
        {
            StickerList.Add(new StickerInstance(bonusInstanceSave.StickerList[i]));
        }
    }
}

[System.Serializable]
public class BonusInstanceSave
{
    public string BonusDataName;
    public List<StickerInstanceSave> StickerList = new();

    public BonusInstanceSave(BonusInstance bonusInstance)
    {
        BonusDataName = bonusInstance.Data.Name;
        for (int i = 0; i < bonusInstance.StickerList.Count; i++)
        {
            StickerList.Add(new StickerInstanceSave(bonusInstance.StickerList[i]));
        }
    }
}

[System.Serializable]
public class StickerInstance
{
    public StickerData Data;
    public Vector3 Position;

    public StickerInstance(StickerData data, Vector3 position)
    {
        Data = data;
        Position = position;
    }

    public StickerInstance(StickerInstanceSave stickerInstanceSave)
    {
        Data = StickerDirector.Instance.StickerDataDictionary[stickerInstanceSave.StickerDataName];
        Position = stickerInstanceSave.Position;
    }
}

[System.Serializable]
public class StickerInstanceSave
{
    public string StickerDataName;
    public Vector3 Position;

    public StickerInstanceSave(StickerInstance stickerInstance)
    {
        StickerDataName = stickerInstance.Data.Name;
        Position = stickerInstance.Position;
    }
}