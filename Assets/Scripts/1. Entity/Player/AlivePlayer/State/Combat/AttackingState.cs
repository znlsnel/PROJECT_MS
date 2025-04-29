using UnityEngine;
using UnityEngine.InputSystem;

public class AttackingState : AlivePlayerCombatState
{
    public AttackingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.AttackingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.AttackingParameterHash);
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack", 1);

        if(normalizedTime >= 0.4f && normalizedTime < 0.45f)
        {
            stateMachine.Player.WeaponHandler.SetIsAttacking(true);
        }
        else if(normalizedTime >= 0.8f)
        {
            stateMachine.Player.WeaponHandler.SetIsAttacking(false);
            combatStateMachine.ChangeState(combatStateMachine.CombatIdlingState);
        }
    }
    #endregion
    
    #region Input Methods
    protected override void OnInputAttack(InputAction.CallbackContext context)
    {
        
    }
    #endregion
}
