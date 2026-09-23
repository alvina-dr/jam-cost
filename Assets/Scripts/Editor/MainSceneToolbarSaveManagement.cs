using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneToolbarSaveManagement
{
    [MainToolbarElement("Custom/New Save", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement ProjectSettingsButton()
    {
        var icon = EditorGUIUtility.IconContent("d_winbtn_mac_close_a").image as Texture2D;
        var content = new MainToolbarContent("New save", icon, "Erase your current save");
        return new MainToolbarButton(content, () => { EraseSave(); });
    }

    static void EraseSave()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.CreateSave();
        }
        
        System.IO.File.Delete(Application.persistentDataPath + "/Save.json");
    }
}
