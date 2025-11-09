using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MND_Scavenge_Classic", menuName = "Scriptable Objects/MapNode/MND_Scavenge_Classic")]
public class MND_Scavenge_Classic : MapNodeData
{
    public int ScoreGoal;
    public int RoundNumber;
    public SpawnItemParameters SpawnItemParameters;
}
