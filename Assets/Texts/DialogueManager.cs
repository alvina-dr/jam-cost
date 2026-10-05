using MoreMountains.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPEffects.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager Instance { get; private set; }

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

    public DialogueRunner DialogueRunner;
    public event Action EndDialogueEvent;
    public GameObject CharacterPicture;
    public Image CharacterPictureImage;

    [SerializedDictionary("Name", "Sprite")] public SerializedDictionary<string, Sprite> CharacterSpriteDictionary = new();

    private void OnAwake()
    {
        List<Sprite> dialogCharacterSpriteList = Resources.LoadAll<Sprite>("DialogCharacterSprite").ToList();
        for (int i = 0; i < dialogCharacterSpriteList.Count; i++)
        {
            CharacterSpriteDictionary.Add(dialogCharacterSpriteList[i].name, dialogCharacterSpriteList[i]);
        }
    }

    public void CallEndDialogueEvent()
    {
        EndDialogueEvent?.Invoke();
        EndDialogueEvent = null;
    }

    [YarnCommand]
    public static void SetCharacterPicture(string character = "")
    {
        if (character == "" || !Instance.CharacterSpriteDictionary.ContainsKey(character))
        {
            Instance.CharacterPicture.gameObject.SetActive(false);
        }
        else
        {
            Instance.CharacterPicture.gameObject.SetActive(true);
            Instance.CharacterPictureImage.sprite = Instance.CharacterSpriteDictionary[character];
        }
    }
}
