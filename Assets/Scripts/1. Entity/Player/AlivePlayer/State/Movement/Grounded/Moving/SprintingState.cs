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

        UpdateCamera(10f, .5f).Forget();

        StartAnimation(stateMachine.Player.AnimationData.SprintingParameterHash);

        
    }

    public override void Exit()
    {
        base.Exit();

        UpdateCamera(8f, .5f).Forget();

        StopAnimation(stateMachine.Player.AnimationData.SprintingParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
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

    async UniTaskVoid UpdateCamera(float targetOffsetY, float duration)
    {
        CinemachineOrbitalFollow camera = stateMachine.Player.CinemachineCamera.GetComponent<CinemachineOrbitalFollow>();

        float elapsedTime = 0f;
        float startOffsetY = camera.TargetOffset.y;

        while(elapsedTime < duration)
        {
            camera.TargetOffset.y = Mathf.Lerp(startOffsetY, targetOffsetY, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        camera.TargetOffset.y = targetOffsetY;
    }
}
