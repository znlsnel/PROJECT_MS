using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
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
        
        stateMachine.ReusableData.MovementSpeed = stateMachine.Player.AlivePlayerSO.SprintSpeed;

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

        if(!stateMachine.ReusableData.ShouldSprint || stateMachine.Player.Stamina.Current <= 0)
        {
            movementStateMachine.ChangeState(movementStateMachine.RunningState);
            return;
        }
    }
    #endregion

    #region Reusable Methods
    protected override void UpdateStamina()
    {
        stateMachine.Player.Stamina.Subtract(Time.deltaTime * 10);
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
