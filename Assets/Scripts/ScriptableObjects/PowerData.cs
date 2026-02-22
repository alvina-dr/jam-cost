using UnityEngine;

[CreateAssetMenu(fileName = "PowerData", menuName = "Scriptable Objects/PowerData")]
public class PowerData : ScriptableObject
{
    public string PowerName;
    public string PowerDescription;
    public string PowerLore;
    public Sprite PowerSprite;
    public int PowerPrice;
    public PowerBehavior PowerBehaviorPrefab;
    public float LoadingTime;
    public float CurrentLoadTime = 0;

    public void Reset()
    {
        CurrentLoadTime = 0;        
    }
}
