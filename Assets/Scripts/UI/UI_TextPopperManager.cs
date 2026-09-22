using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_TextPopperManager : MonoBehaviour
{
    [SerializeField] private UI_TextPopper _textPopperPrefab;
    [SerializeField] private Transform _textPopperParent;

    [SerializeField] private List<UI_TextPopper> _pool = new();


    [Button]
    public void PopText(string text, Vector3 position, Color color = default, UI_TextPopper.AnimSpeed speed = UI_TextPopper.AnimSpeed.Normal)
    {
        UI_TextPopper textPopper = GetPoolItem();
        textPopper.gameObject.SetActive(true);
        textPopper.transform.position = position;
        textPopper.PopText(text, color, speed);
    }

    public UI_TextPopper GetPoolItem()
    {
        UI_TextPopper textPopper = _pool.Find(x => x.gameObject.activeSelf == false);

        if (textPopper == null) 
        {
            textPopper = Instantiate(_textPopperPrefab, _textPopperParent);
            _pool.Add(textPopper);
        }

        return textPopper;
    }
}
