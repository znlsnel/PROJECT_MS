using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SprintingState : MovingState
{
    private static readonly string _clickSound = "Sound/Player/Footstep_01_01.mp3";
    private Coroutine _footstepCoroutine;
    private const float FOOTSTEP_INTERVAL = 0.2f;

    public SprintingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        stateMachine.ReusableData.MovementSpeed = stateMachine.Player.AlivePlayerSO.SprintSpeed;

        StartAnimation(stateMachine.Player.AnimationData.SprintingParameterHash);
        
        _footstepCoroutine = stateMachine.Player.StartCoroutine(PlayFootstepRoutine());
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.SprintingParameterHash);
        
        if (_footstepCoroutine != null)
        {
            stateMachine.Player.StopCoroutine(_footstepCoroutine);
            _footstepCoroutine = null;
        }
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
            return;
        }

        if(!stateMachine.ReusableData.ShouldSprint || stateMachine.Player.Stamina.Current.Value <= 0)
        {
            movementStateMachine.ChangeState(movementStateMachine.RunningState);
            return;
        }
    }
    #endregion

    private IEnumerator PlayFootstepRoutine()
    {
        while (true)
        {
            Managers.Sound.Play(_clickSound);
            yield return new WaitForSeconds(FOOTSTEP_INTERVAL);
        }
    }

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
