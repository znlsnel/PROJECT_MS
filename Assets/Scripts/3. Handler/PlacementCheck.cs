using UnityEngine;

public class PlacementCheck : MonoBehaviour
{
    [Header("기울기 제한 여부")]
    public bool enforceSlopeLimit = true;

    [Header("기울기 각도")]
    [Range(0f, 90f)]
    public float maxSlopeAngle = 30f;

    [Header("겹침 검사 여부")]
    public bool enforceOverlapCheck = true;
} 