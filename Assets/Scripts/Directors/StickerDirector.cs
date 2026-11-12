using System.Collections.Generic;
using System.Linq;
using TMPEffects.SerializedCollections;
using UnityEngine;

public class StickerDirector : MonoBehaviour
{
    #region Singleton
    public static StickerDirector Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            OnAwake();
        }
    }
    #endregion

    [SerializedDictionary("Name", "StickerData")] public SerializedDictionary<string, StickerData> StickerDataDictionary = new();

    private void OnAwake()
    {
        List<StickerData> stickerDataList = Resources.LoadAll<StickerData>("StickerData").ToList();
        stickerDataList = stickerDataList.FindAll(x => x.IsAvailableInGame);
        for (int i = 0; i < stickerDataList.Count; i++)
        {
            StickerData stickerData = Instantiate(stickerDataList[i]);
            stickerData.name = stickerData.Name;
            StickerDataDictionary.Add(stickerData.Name, stickerData);
        }
    }

    public static StickerData GetRandomSticker()
    {
        List<StickerData> stickerDataList = Instance.StickerDataDictionary.Values.ToList();
        StickerData sticker = stickerDataList[Random.Range(0, stickerDataList.Count)];
        return sticker;
    }
}
