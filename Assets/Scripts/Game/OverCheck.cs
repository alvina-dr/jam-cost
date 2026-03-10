using UnityEngine;

public class OverCheck : MonoBehaviour
{
    public bool MouseOver;
    [SerializeField] private LayerMask _layerMask;

    public void OnMouseEnter()
    {
        MouseOver = true;
    }

    public void OnMouseExit() 
    { 
        MouseOver = false;
    }

    public bool IsOver()
    {
        Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), layerMask: _layerMask);
        if (collider != null)
        {
            if (collider.transform == transform) return true;
        }

        return false;
    }
}
