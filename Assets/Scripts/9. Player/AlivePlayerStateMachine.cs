using UnityEngine;

public class AlivePlayerStateMachine : StateMachine
{
    public AlivePlayer Player { get; private set; }
    public AlivePlayerStateReusableData ReusableData { get; private set; }

    public IdlingState IdlingState { get; private set; }
    public InterctingState InterctingState { get; private set; }
    public RunningState RunningState { get; private set; }
    public SprintingState SprintingState { get; private set; }
    public AttackingState AttackingState { get; private set; }
    public AimingState AimingState { get; private set; }
    public DamagedState DamagedState { get; private set; }
    public DeadState DeadState { get; private set; }

    public AlivePlayerStateMachine(AlivePlayer player)
    {
        Player = player;
        ReusableData = new AlivePlayerStateReusableData();

        IdlingState = new IdlingState(this);
        InterctingState = new InterctingState(this);

        RunningState = new RunningState(this);
        SprintingState = new SprintingState(this);

        AttackingState = new AttackingState(this);
        AimingState = new AimingState(this);
        DamagedState = new DamagedState(this);
        DeadState = new DeadState(this);

        ChangeState(IdlingState);
    }
}
