using UnityEngine;

public class AlivePlayerStateReusableData
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier => MovementSpeed * MovementSpeedPercentage;
    public float MovementSpeed { get; set; } = 1f;
    public float MovementSpeedPercentage { get; set; } = 1f;

    public Vector3 VerticalVelocity { get; set; }

    public bool ShouldSprint { get; set; }
    public bool IsDead { get; set; } = false;
}
