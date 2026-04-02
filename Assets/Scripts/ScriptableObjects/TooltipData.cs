using UnityEngine;

[CreateAssetMenu(fileName = "TooltipData", menuName = "Scriptable Objects/TooltipData")]
public class TooltipData : ScriptableObject
{
    [TextArea] public string Description;
}
