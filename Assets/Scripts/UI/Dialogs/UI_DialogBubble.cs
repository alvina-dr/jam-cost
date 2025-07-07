using System;
using TMPro;
using UnityEngine;

public class UI_DialogBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private AudioClip _pop;

    public void Setup(DialogData.LineData line)
    {
        AudioManager.Instance.PlaySFXSound(_pop);
        _nameText.text = line.Name;
        _dialogText.text = line.Dialog;
    }
}
