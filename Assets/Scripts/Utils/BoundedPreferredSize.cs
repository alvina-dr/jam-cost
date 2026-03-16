using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * Original implementation due to @Mizzrym1
 * https://discussions.unity.com/t/setting-maximum-width-of-layout-element-with-content-size-fitter/586475/21
 */

[ExecuteInEditMode]
public sealed class BoundedPreferredSize : MonoBehaviour, ILayoutElement, ILayoutIgnorer
{
    [SerializeField] private Component m_layoutElementComponent;
    [SerializeField] private bool m_IgnoreLayout = false;
    [SerializeField] private float m_MinWidth = -1;
    [SerializeField] private float m_MinHeight = -1;
    [SerializeField] private float m_MaxWidth = -1;
    [SerializeField] private float m_MaxHeight = -1;
    [SerializeField] private int m_layoutPriority = 1;

    private ILayoutElement layoutElement => m_layoutElementComponent as ILayoutElement;

    private void OnValidate()
    {
        if (m_layoutElementComponent == null || m_layoutElementComponent is ILayoutElement) return;
        Debug.LogWarning("The assigned component does not implement ILayoutElement.", this);
        m_layoutElementComponent = null;
    }

    public void CalculateLayoutInputHorizontal() => layoutElement?.CalculateLayoutInputHorizontal();

    public void CalculateLayoutInputVertical() => layoutElement?.CalculateLayoutInputVertical();

    public float minWidth => -1;

    public float preferredWidth => Mathf.Clamp(layoutElement != null ? layoutElement.preferredWidth : 0, m_MinWidth, m_MaxWidth);

    public float flexibleWidth => -1;

    public float minHeight => -1;

    public float preferredHeight => Mathf.Clamp(layoutElement != null ? layoutElement.preferredHeight : 0, m_MinHeight, m_MaxHeight);

    public float flexibleHeight => -1;

    public int layoutPriority => m_layoutPriority;

    public bool ignoreLayout => m_IgnoreLayout;
}

#if UNITY_EDITOR
[CustomEditor(typeof(BoundedPreferredSize))]
public class BoundedPreferredSizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif