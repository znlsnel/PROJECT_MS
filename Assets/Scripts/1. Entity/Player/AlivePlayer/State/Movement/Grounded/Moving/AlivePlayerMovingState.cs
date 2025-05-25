using UnityEngine;

public class AlivePlayerMovingState : AlivePlayerGroundedState
{
    public AlivePlayerMovingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        
        StartAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
    }
    #endregion
}
