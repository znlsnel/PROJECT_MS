using System;
using UnityEngine;

[Serializable]
public class AlivePlayerAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundParameterHashName = "Ground";
    [SerializeField] private string movingParameterHashName = "Moving";
    [SerializeField] private string combatParameterHashName = "Combat";

    [Header("State Moving Parameter Names")]
    [SerializeField] private string idlingParameterHashName = "isIdling";
    [SerializeField] private string runningParameterHashName = "isRunning";
    [SerializeField] private string sprintingParameterHashName = "isSprinting";
    [SerializeField] private string interactParameterHashName = "isInteracting";

    [Header("State Combat Parameter Names")]
    [SerializeField] private string aimingParameterHashName = "isAiming";
    [SerializeField] private string attackingParameterHashName = "isAttacking";
    [SerializeField] private string damagedParameterHashName = "isDamaged";
    [SerializeField] private string deadParameterHashName = "isDead";

    public int GroundParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int CombatParameterHash { get; private set; }

    public int IdlingParameterHash { get; private set; }
    public int RunningParameterHash { get; private set; }
    public int SprintingParameterHash { get; private set; }
    public int InteractParameterHash { get; private set; }

    public int AimingParameterHash { get; private set; }
    public int AttackingParameterHash { get; private set; }
    public int DamagedParameterHash { get; private set; }
    public int DeadParameterHash { get; private set; }

    public AlivePlayerAnimationData()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterHashName);
        MovingParameterHash = Animator.StringToHash(movingParameterHashName);
        CombatParameterHash = Animator.StringToHash(combatParameterHashName);
        
        IdlingParameterHash = Animator.StringToHash(idlingParameterHashName);
        RunningParameterHash = Animator.StringToHash(runningParameterHashName);
        SprintingParameterHash = Animator.StringToHash(sprintingParameterHashName);
        InteractParameterHash = Animator.StringToHash(interactParameterHashName);

        AimingParameterHash = Animator.StringToHash(aimingParameterHashName);
        AttackingParameterHash = Animator.StringToHash(attackingParameterHashName);
        DamagedParameterHash = Animator.StringToHash(damagedParameterHashName);
        DeadParameterHash = Animator.StringToHash(deadParameterHashName);
    }
}
