using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class MainSceneToolbarButton
{
	static MainSceneToolbarButton()
	{
		ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
		ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
    }

    static void OpenChosenScene(object obj)
    {
        EditorSceneManager.OpenScene($"Assets/Scenes/{obj}.unity");
    }

    static void OnLeftToolbarGUI()
	{
		GUILayout.FlexibleSpace();
        GenericMenu dropdownMenu = new GenericMenu();

        dropdownMenu.AddItem(new GUIContent("Game"), false, OpenChosenScene, "Game");
        dropdownMenu.AddItem(new GUIContent("Main Menu"), false, OpenChosenScene, "MainMenu");
        dropdownMenu.AddItem(new GUIContent("Shop"), false, OpenChosenScene, "Shop");
        dropdownMenu.AddItem(new GUIContent("Map"), false, OpenChosenScene, "Map");
        dropdownMenu.AddItem(new GUIContent("Hub"), false, OpenChosenScene, "Hub");
        dropdownMenu.AddItem(new GUIContent("Office"), false, OpenChosenScene, "Office");
        dropdownMenu.AddItem(new GUIContent("Possession"), false, OpenChosenScene, "Boss/Possession");
        dropdownMenu.AddItem(new GUIContent("Free Round"), false, OpenChosenScene, "FreeRound");
        dropdownMenu.AddItem(new GUIContent("Onboarding"), false, OpenChosenScene, "Onboarding");

        if (EditorGUILayout.DropdownButton(new GUIContent("Load scene"), FocusType.Keyboard))
        {
            dropdownMenu.ShowAsContext();
        }
    }

    static void OnRightToolbarGUI()
    {
        if (GUILayout.Button(new GUIContent("New save", "Creates a new save")))
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.CreateSave();
            }

            System.IO.File.Delete(Application.persistentDataPath + "/Save.json");
        }
    }
}
