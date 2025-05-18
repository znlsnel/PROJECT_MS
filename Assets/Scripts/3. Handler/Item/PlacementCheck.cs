using UnityEngine;
using FishNet.Object;

public class PlacementCheck
{
    // "기울기 제한 여부"
    public bool slopeLimit = true;


    // "기울기 각도" (0f, 90f)
    public float maxSlopeAngle = 30f;

    // "겹침 검사 여부"
    public bool overlapCheck = true;

    public PlacementCheck() {}
    public PlacementCheck(ItemData itemData)
    {
        slopeLimit = itemData.SlopeLimit;
        maxSlopeAngle = itemData.MaxSlopeAngle;
        overlapCheck = itemData.OverlapCheck;
    }
}