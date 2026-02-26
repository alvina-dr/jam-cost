using Sirenix.OdinInspector;
using UnityEngine;

public class UI_TextPopperManager : MonoBehaviour
{
    [SerializeField] private UI_TextPopper _textPopperPrefab;
    [SerializeField] private Transform _textPopperParent;


    [Button]
    public void PopText(string text, Vector3 position, Color color = default, UI_TextPopper.AnimSpeed speed = UI_TextPopper.AnimSpeed.Normal)
    {
        UI_TextPopper textPopper = Instantiate(_textPopperPrefab, _textPopperParent);
        textPopper.transform.position = position;
        textPopper.PopText(text, color, speed);
    }
}
