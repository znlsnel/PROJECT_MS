using UnityEngine;

public class AlivePlayerMovementStateMachine : StateMachine
{
    public AlivePlayerStateMachine stateMachine { get; private set; }

    public AlivePlayerIdlingState IdlingState { get; private set; }
    public AlivePlayerInterctingState InterctingState { get; private set; }
    public AlivePlayerRunningState RunningState { get; private set; }
    public AlivePlayerSprintingState SprintingState { get; private set; }
    public AlivePlayerDeadState DeadState { get; private set; }

    public AlivePlayerMovementStateMachine(AlivePlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Init()
    {
        IdlingState = new AlivePlayerIdlingState(stateMachine);
        InterctingState = new AlivePlayerInterctingState(stateMachine);

        RunningState = new AlivePlayerRunningState(stateMachine);
        SprintingState = new AlivePlayerSprintingState(stateMachine);

        DeadState = new AlivePlayerDeadState(stateMachine);

        ChangeState(IdlingState);
    }
}
