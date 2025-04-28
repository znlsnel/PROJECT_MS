using UnityEngine;

public class CombatIdlingState : AlivePlayerCombatState
{
    public CombatIdlingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion
}
