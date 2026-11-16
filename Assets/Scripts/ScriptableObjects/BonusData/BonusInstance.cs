using PrimeTween;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonusInstance
{
    public BonusData Data;
    public List<StickerInstance> StickerList = new();

    public void AddSticker(StickerBehavior stickerBehavior, AppliedStickerBehavior appliedSticker)
    {
        StickerList.Add(appliedSticker.StickerInstance);
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

    public void CheckSticker(BonusBehavior bonusBehavior)
    {
        for (int i = 0; i < StickerList.Count; i++)
        {
            StickerList[i].Data.CheckSticker(bonusBehavior);
        }
    }

    public T GetStickerDataByType<T>() where T : StickerData
    {
        StickerInstance stickerInstance = StickerList.Find(x => x.Data is T);
        if (stickerInstance == null) return null;
        return stickerInstance.Data as T;
    }

    public StickerInstance GetStickerInstanceByName(string stickerName)
    {
        StickerInstance stickerInstance = StickerList.Find(x => x.Data.Name == stickerName);
        return stickerInstance;
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