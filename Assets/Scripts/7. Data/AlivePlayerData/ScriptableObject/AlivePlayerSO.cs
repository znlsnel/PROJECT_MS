using UnityEngine;

[CreateAssetMenu(fileName = "AlivePlayerSO", menuName = "Player/AlivePlayerSO", order = -9999)]
public class AlivePlayerSO : ScriptableObject
{
    [field: SerializeField] [field: Range(0, 100)] public float MoveSpeed { get; private set; } = 3;
    [field: SerializeField] [field: Range(0, 100)] public float SprintSpeed { get; private set; } = 5;
    [field: SerializeField] [field: Range(0, 100)] public float RotationSpeed { get; private set; } = 10;
}
