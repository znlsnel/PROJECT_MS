using UnityEngine;
using UnityEngine.InputSystem;

public class SprintingState : MovingState
{
    public SprintingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        stateMachine.ReusableData.MovementSpeedModifier = stateMachine.Player.AlivePlayerSO.SprintSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion

    #region Input Methods
    protected override void OnSprintCanceled(InputAction.CallbackContext context)
    {
        
    }
    #endregion
}
