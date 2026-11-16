using UnityEngine;

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