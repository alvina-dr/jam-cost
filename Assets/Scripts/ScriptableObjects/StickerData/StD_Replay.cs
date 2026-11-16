using PrimeTween;
using UnityEngine;

[CreateAssetMenu(fileName = "StD_Replay", menuName = "Scriptable Objects/StickerData/StD_Replay")]
public class StD_Replay : StickerData
{
    public override bool CheckSticker(BonusBehavior bonusBehavior)
    {
        return false;
    }
}
