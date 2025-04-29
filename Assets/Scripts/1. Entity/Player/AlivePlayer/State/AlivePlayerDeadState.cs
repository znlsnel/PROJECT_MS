using UnityEngine;

public class AlivePlayerDeadState : AlivePlayerState
{
    public AlivePlayerDeadState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"StateMachine : Enter {GetType().Name}");
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }
}
