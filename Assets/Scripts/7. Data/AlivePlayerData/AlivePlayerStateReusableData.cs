using UnityEngine;

public class AlivePlayerStateReusableData
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public Vector3 VerticalVelocity { get; set; }

    public bool ShouldSprint { get; set; }
}
