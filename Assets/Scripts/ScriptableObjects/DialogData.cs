using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/DialogData")]
public class DialogData : ScriptableObject
{
    public List<LineData> LineDataList = new();

    [System.Serializable]
    public class LineData
    {
        public string Name;
        public string Dialog;
        public Side DialogSide;
        public float Interval;

        public enum Side
        {
            Left = 0,
            Right = 1
        }
    }
}
