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
        stateMachine.Player.InteractionHandler.onInputInteract += OnInteract;

        base.Enter();
        
        stateMachine.ReusableData.VerticalVelocity = Physics.gravity;

        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        stateMachine.Player.InteractionHandler.onInputInteract -= OnInteract;

        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    #endregion

    #region Reusable Methods
    protected virtual void OnMove()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return;
        }

        if(stateMachine.ReusableData.ShouldSprint)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }
    #endregion

    #region Input Methods
    protected virtual void OnInteract()
    {
        Interactable interactObject = stateMachine.Player.InteractionHandler.GetInteractObject();
        SetInteractAnimation(interactObject.interactAnimation, interactObject.interactAnimationSpeed);
        stateMachine.ChangeState(stateMachine.InterctingState);
    }
    #endregion
}
