using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class StoryItemDirector : MonoBehaviour
{
    #region Singleton
    public static StoryItemDirector Instance { get; private set; }

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

    public Dictionary<string, StoryItemData> StoryItemDataDictionary = new();

    private void OnAwake()
    {
        List<StoryItemData> storyItemDataList = Resources.LoadAll<StoryItemData>("StoryItemData").ToList();
        for (int i = 0; i < storyItemDataList.Count; i++)
        {
            StoryItemData storyItemData = Instantiate(storyItemDataList[i]);
            storyItemData.name = storyItemData.Name;
            StoryItemDataDictionary.Add(storyItemData.Name, storyItemData);
        }
    }

    public void GetStoryItem(StoryItemData storyItemData)
    {
        SaveManager.Instance.StoryItemDataList.Add(StoryItemDataDictionary[storyItemData.Name]);
    }

    [YarnFunction]
    public static bool CheckHasStoryItem(string storyItemName)
    {
        if (!Instance.StoryItemDataDictionary.ContainsKey(storyItemName)) return false;
        return SaveManager.Instance.StoryItemDataList.Contains(Instance.StoryItemDataDictionary[storyItemName]);
    }

    [YarnFunction]
    public static bool CheckHasAnyStoryItem()
    {
        return SaveManager.Instance.StoryItemDataList.Count > 0;
    }
}
