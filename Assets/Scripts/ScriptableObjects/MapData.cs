using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
public class MapData : ScriptableObject
{
    public List<MapNodeDailyChoice> DailyChoiceList = new();

    [System.Serializable]
    public class MapNodeDailyChoice
    {
        public List<MapNodeData> MapNodeDataList = new();
    }

    public MapNodeData GetRandomMapNodeAtDay(int day)
    {
        if (day >= DailyChoiceList.Count) return null;
        return DailyChoiceList[day].MapNodeDataList[Random.Range(0, DailyChoiceList[day].MapNodeDataList.Count)];
    }
}
