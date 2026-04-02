using UnityEngine;
using UnityEngine.UI;

public class UI_Rebuilder : MonoBehaviour
{
    [SerializeField] RectTransform _layout;
    [SerializeField] bool _rebuildOnStart;

    void Start()
    {
        if (_rebuildOnStart)
        {
            ForceRebuildLayoutImmediate();
        }
    }

    public void ForceRebuildLayoutImmediate()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_layout);
    }
}
