using System;
using TMPro;
using UnityEngine;

public class UI_DialogBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogText;

    public void Setup(DialogData.LineData line)
    {
        _nameText.text = line.Name;
        _dialogText.text = line.Dialog;
    }
}
