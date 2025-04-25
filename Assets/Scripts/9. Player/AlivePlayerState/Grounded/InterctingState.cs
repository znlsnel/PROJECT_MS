using UnityEngine;

public class InterctingState : GroundedState
{
    public InterctingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0f;

        base.Enter();
        
        StartAnimation(stateMachine.Player.AnimationData.InteractParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(stateMachine.Player.AnimationData.InteractParameterHash);

        stateMachine.Player.SetInteractAnimation(null);
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Interacting");

        if(normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }
    #endregion
}
