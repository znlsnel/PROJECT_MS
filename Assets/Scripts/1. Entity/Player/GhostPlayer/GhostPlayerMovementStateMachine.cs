using UnityEngine;

public class GhostPlayerMovementStateMachine : StateMachine
{
    public GhostPlayerStateMachine stateMachine { get; private set; }
    public GhostPlayerIdlingState IdlingState { get; private set; }
    public GhostPlayerRunningState RunningState { get; private set; }
    public GhostPlayerSprintingState SprintingState { get; private set; }
    public GhostPlayerMovementStateMachine(GhostPlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public void Init()
    {
        IdlingState = new GhostPlayerIdlingState(stateMachine);
        RunningState = new GhostPlayerRunningState(stateMachine);
        SprintingState = new GhostPlayerSprintingState(stateMachine);
        ChangeState(IdlingState);
    }
}
