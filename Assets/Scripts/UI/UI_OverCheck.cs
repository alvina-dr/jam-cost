using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OverCheck : MonoBehaviour
{
    [SerializeField] private Image _image;

    public bool IsOver()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count == 0) return false;

        for (int i = 0; i < results.Count; i++)
        {
            //Debug.Log(results[i].gameObject.name);
            if (results[i].gameObject == gameObject)
            {
                results.RemoveAt(i);
                i--;
            }
        }

        return results.Count == 0;
    }

}
