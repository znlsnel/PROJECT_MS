using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class GhostPlayerSprintingState : GhostPlayerMovingState
{
    public GhostPlayerSprintingState(GhostPlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        stateMachine.ReusableData.MovementSpeed = stateMachine.Player.GhostPlayerSO.SprintSpeed;
        StartAnimation(stateMachine.Player.AnimationData.SprintingParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SprintingParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
            return;
        }
        if(!stateMachine.ReusableData.ShouldSprint)
        {
            movementStateMachine.ChangeState(movementStateMachine.RunningState);
            return;
        }
    }
    #endregion
    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
        
        base.OnMovementCanceled(context);
    }
    #endregion
}

