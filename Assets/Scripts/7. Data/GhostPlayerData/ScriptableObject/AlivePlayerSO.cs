using UnityEngine;

[CreateAssetMenu(fileName = "GhostPlayerSO", menuName = "Player/GhostPlayerSO", order = -9999)]
public class GhostPlayerSO : ScriptableObject
{
    [field: SerializeField] [field: Range(0, 100)] public float MoveSpeed { get; private set; } = 3;
    [field: SerializeField] [field: Range(0, 100)] public float SprintSpeed { get; private set; } = 5;
    [field: SerializeField] [field: Range(0, 100)] public float RotationSpeed { get; private set; } = 10;
}
