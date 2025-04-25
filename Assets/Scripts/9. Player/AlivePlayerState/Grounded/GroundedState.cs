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
        stateMachine.Player.InteractionHandler.OnInteract += OnInteract;

        base.Enter();
        
        stateMachine.ReusableData.VerticalVelocity = Physics.gravity;

        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        stateMachine.Player.InteractionHandler.OnInteract -= OnInteract;

        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
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

    protected virtual void OnInteract(GameObject gameObject)
    {
        stateMachine.ChangeState(stateMachine.InterctingState);
    }
    #endregion
}
