using System.Collections.Generic;
using System.Linq;
using TMPEffects.SerializedCollections;
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

    [SerializedDictionary("Name", "StoryItemData")] public SerializedDictionary<string, StoryItemData> StoryItemDataDictionary = new();

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

    public void AddAllRelationLevel()
    {
        List<string> characterNameList = SaveManager.CurrentSave.CharacterLevelDictionary.Keys.ToList();
        for (int i = 0; i < characterNameList.Count; i++)
        {
            StoryItemDirector.AddRelationLevel(characterNameList[i]);
        }
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

    [YarnCommand]
    public static void GiveStoryItemToNPC(string storyItemName)
    {
        if (!Instance.StoryItemDataDictionary.ContainsKey(storyItemName)) return;
        SaveManager.Instance.StoryItemDataList.Remove(Instance.StoryItemDataDictionary[storyItemName]);
    }

    [YarnCommand]
    public static void AddRelationLevel(string character)
    {
        if (!SaveManager.CurrentSave.CharacterLevelDictionary.ContainsKey(character))
        {
            SaveManager.CurrentSave.CharacterLevelDictionary.Add(character, 0);
        }

        SaveManager.CurrentSave.CharacterLevelDictionary[character]++;
    }

    [YarnFunction]
    public static int GetRelationLevel(string character)
    {
        if (!SaveManager.CurrentSave.CharacterLevelDictionary.ContainsKey(character))
        {
            SaveManager.CurrentSave.CharacterLevelDictionary.Add(character, 0);
        }

        return SaveManager.CurrentSave.CharacterLevelDictionary[character];
    }

    [YarnFunction]
    public static int GetNumberOfRun()
    {
        return SaveManager.CurrentSave.RunDataHistory.Count;
    }
}
