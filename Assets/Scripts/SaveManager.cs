using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            OnAwake();
        }
    }
    #endregion

    public MapNodeData CurrentMapNode;

    private void OnAwake()
    {

    }

    public MND_ClassicScavenge GetClassicScavengeNode()
    {
        return (MND_ClassicScavenge) CurrentMapNode;
    }

    public class SaveData
    {
        
    }
}
