using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    #region Singleton
    public static GameDirector Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public List<RarityWeight> RarityWeight = new();

    private void Start()
    {
        
    }

    public Rarity GetRandomRarity()
    {
        int totalWeight = 0;
        for (int i = 0; i < RarityWeight.Count; i++)
        {
            totalWeight += RarityWeight[i].Weight;
        }

        int random = Random.Range(0, totalWeight);
        totalWeight = 0;
        for (int i = 0; i < RarityWeight.Count; i++)
        {
            totalWeight += RarityWeight[i].Weight;
            if (random < totalWeight)
            {
                return RarityWeight[i].Rarity;
            }
        }

        return Rarity.Common;
    }
}

[System.Serializable]
public class RarityWeight
{
    public int Weight;
    public Rarity Rarity;
}

public enum Rarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Impossible = 3
}
