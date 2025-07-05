using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData", menuName = "Scriptable Objects/RoundData")]
public class RoundData : ScriptableObject
{
    public List<Round> RoundDataList = new();


    [Serializable]
    public class Round
    {
        public int ItemSpawnNumber;
        public int ScoreGoal;
    }
}
