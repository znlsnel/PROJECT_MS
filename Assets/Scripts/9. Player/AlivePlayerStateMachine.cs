using UnityEngine;

public class AlivePlayerStateMachine : StateMachine
{
    public AlivePlayer Player { get; private set; }

    public IdlingState idlingState { get; private set; }
    public InterctingState interctingState { get; private set; }
    public RunningState runningState { get; private set; }
    public SprintingState sprintingState { get; private set; }
    public AttackingState attackingState { get; private set; }
    public AimingState aimingState { get; private set; }
    public DamagedState damagedState { get; private set; }
    public DeadState deadState { get; private set; }

    public AlivePlayerStateMachine(AlivePlayer player)
    {
        Player = player;

        idlingState = new IdlingState(this);
        interctingState = new InterctingState(this);

        runningState = new RunningState(this);
        sprintingState = new SprintingState(this);

        attackingState = new AttackingState(this);
        aimingState = new AimingState(this);
        damagedState = new DamagedState(this);
        deadState = new DeadState(this);

        ChangeState(idlingState);
    }
}
