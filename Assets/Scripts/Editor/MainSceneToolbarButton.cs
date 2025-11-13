using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbarExtender;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class MainSceneToolbarButton
{
	static MainSceneToolbarButton()
	{
		ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
	}
	
	static void OnToolbarGUI()
	{
		GUILayout.FlexibleSpace();

		if (GUILayout.Button(new GUIContent("Game", "Open Game Scene")))
		{
			EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
		}

        if (GUILayout.Button(new GUIContent("Shop", "Open Shop Scene")))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Shop.unity");
        }

        if (GUILayout.Button(new GUIContent("Map", "Open Map Scene")))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Map.unity");
        }

        if (GUILayout.Button(new GUIContent("Main Menu", "Open Main Menu Scene")))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        }
    }
}
