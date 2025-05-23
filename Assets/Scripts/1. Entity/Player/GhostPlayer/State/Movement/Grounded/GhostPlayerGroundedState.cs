using UnityEngine;
using UnityEngine.InputSystem;


public class GhostPlayerGroundedState : GhostPlayerMovementState
{
    public GhostPlayerGroundedState(GhostPlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        stateMachine.ReusableData.VerticalVelocity = Physics.gravity;
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    #endregion
    #region Reusable Methods
    protected virtual bool OnMove()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return false;
        }
        if(stateMachine.ReusableData.ShouldSprint)
        {
            movementStateMachine.ChangeState(movementStateMachine.SprintingState);
            return true;
        }
        movementStateMachine.ChangeState(movementStateMachine.RunningState);
        return true;
    }
    #endregion
}

