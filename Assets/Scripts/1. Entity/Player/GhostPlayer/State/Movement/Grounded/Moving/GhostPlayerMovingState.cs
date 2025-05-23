using UnityEngine;


public class GhostPlayerMovingState : GhostPlayerGroundedState
{
    public GhostPlayerMovingState(GhostPlayerStateMachine stateMachine) : base(stateMachine)
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

