using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/BonusData")]
public class BonusData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
}
