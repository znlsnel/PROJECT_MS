using UnityEngine;

public class InterctingState : GroundedState
{
    public InterctingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        StartAnimation(stateMachine.Player.AnimationData.InteractParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(stateMachine.Player.AnimationData.InteractParameterHash);
    }
    #endregion
}
