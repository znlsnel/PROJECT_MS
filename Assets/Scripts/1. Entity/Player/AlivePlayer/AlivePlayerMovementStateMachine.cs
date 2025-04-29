using UnityEngine;

public class AlivePlayerMovementStateMachine : StateMachine
{
    public AlivePlayerStateMachine stateMachine { get; private set; }

    public IdlingState IdlingState { get; private set; }
    public InterctingState InterctingState { get; private set; }
    public RunningState RunningState { get; private set; }
    public SprintingState SprintingState { get; private set; }
    public AlivePlayerDeadState DeadState { get; private set; }

    public AlivePlayerMovementStateMachine(AlivePlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Init()
    {
        IdlingState = new IdlingState(stateMachine);
        InterctingState = new InterctingState(stateMachine);

        RunningState = new RunningState(stateMachine);
        SprintingState = new SprintingState(stateMachine);

        DeadState = new AlivePlayerDeadState(stateMachine);

        ChangeState(IdlingState);
    }
}
