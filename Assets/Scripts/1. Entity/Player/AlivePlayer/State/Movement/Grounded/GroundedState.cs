using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : AlivePlayerMovementState
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
    protected virtual bool OnMove()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return false;
        }

        if(stateMachine.ReusableData.ShouldSprint && stateMachine.Player.Stamina.Current.Value > stateMachine.Player.Stamina.Maximum.Value * 0.1f)
        {
            movementStateMachine.ChangeState(movementStateMachine.SprintingState);
            return true;
        }

        movementStateMachine.ChangeState(movementStateMachine.RunningState);
        return true;
    }

    protected override void OnDead()
    {
        movementStateMachine.ChangeState(movementStateMachine.DeadState);
    }
    #endregion

    #region Input Methods
    protected virtual void OnInteract()
    {
        if(stateMachine.CombatStateMachine.currentState == stateMachine.CombatStateMachine.AttackingState)
            return;
        
        if(stateMachine.CombatStateMachine.currentState == stateMachine.CombatStateMachine.AimingState)
            return;

        Interactable interactObject = stateMachine.Player.InteractionHandler.GetInteractObject();
        SetInteractAnimation(interactObject.interactAnimation, interactObject.interactAnimationSpeed);
        movementStateMachine.ChangeState(movementStateMachine.InterctingState);
    }
    #endregion
}
