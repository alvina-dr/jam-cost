using System.Collections.Generic;
using UnityEngine;

public class UI_PowerMenu : MonoBehaviour
{
    [SerializeField] private List<UI_PowerButton> _powerButtonList = new();

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        for (int i = 0; i < _powerButtonList.Count; i++)
        {
            if (i < SaveManager.CurrentSave.EquipedPowerDataList.Count)
            {
                _powerButtonList[i].Setup(SaveManager.CurrentSave.EquipedPowerDataList[i]);
                _powerButtonList[i].gameObject.SetActive(true);
            }
            else
            {
                _powerButtonList[i].gameObject.SetActive(false);
            }
        }
    }
}
