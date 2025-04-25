using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : AlivePlayerState
{
    public GroundedState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        stateMachine.ReusableData.VerticalVelocity = Physics.gravity;
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
        Managers.Input.GetInput(EPlayerInput.Sprint).started += OnSprintCanceled;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();
        Managers.Input.GetInput(EPlayerInput.Sprint).started -= OnSprintCanceled;
    }

    protected virtual void OnMove()
    {
        if(stateMachine.ReusableData.ShouldSprint)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }
    #endregion

    #region Input Methods
    protected virtual void OnSprintCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ReusableData.ShouldSprint = false;
    }
    #endregion
}
