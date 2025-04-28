using UnityEngine;

public class AlivePlayerCombatStateMachine : StateMachine
{
    public AlivePlayerStateMachine stateMachine { get; private set; }

    public CombatIdlingState CombatIdlingState { get; private set; }
    public AttackingState AttackingState { get; private set; }
    public AimingState AimingState { get; private set; }
    public DamagedState DamagedState { get; private set; }
    public DeadState DeadState { get; private set; }
    
    public AlivePlayerCombatStateMachine(AlivePlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Init()
    {
        CombatIdlingState = new CombatIdlingState(stateMachine);
        AttackingState = new AttackingState(stateMachine);
        AimingState = new AimingState(stateMachine);
        DamagedState = new DamagedState(stateMachine);
        DeadState = new DeadState(stateMachine);

        ChangeState(CombatIdlingState);
    }
}
