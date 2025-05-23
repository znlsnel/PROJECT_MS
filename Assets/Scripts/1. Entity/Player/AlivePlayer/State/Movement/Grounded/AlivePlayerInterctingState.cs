using Cysharp.Threading.Tasks;
using UnityEngine;

public class AlivePlayerInterctingState : AlivePlayerGroundedState
{
    private Vector3 direction;
    private float targetRotationYAngle;

    public AlivePlayerInterctingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        Interactable interactObject = stateMachine.Player.InteractionHandler.GetInteractObject();

        direction = interactObject.transform.position - stateMachine.Player.transform.position;

        targetRotationYAngle = GetRotationAngle(direction);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.InteractParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(OnMove()) return;

        float angleDifference = Mathf.Abs(targetRotationYAngle - GetRotationAngle(stateMachine.Player.transform.forward));

        if(angleDifference > 1f)
        {
            RotationPlayer(direction);
            return;
        }

        StartAnimation(stateMachine.Player.AnimationData.InteractParameterHash);

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Interacting");

        if(normalizedTime >= 0.8f)
        {
            stateMachine.Player.InteractionHandler.OnInteract();

            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
        }
    }
    #endregion

    #region Input Methods
    protected override void OnInteract()
    {

    }
    #endregion
}
