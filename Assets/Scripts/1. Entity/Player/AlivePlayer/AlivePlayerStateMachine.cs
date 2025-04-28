using UnityEngine;

public class AlivePlayerStateMachine
{
    public AlivePlayer Player { get; private set; }
    public AlivePlayerStateReusableData ReusableData { get; private set; }

    public AlivePlayerMovementStateMachine MovementStateMachine { get; private set; }
    public AlivePlayerCombatStateMachine CombatStateMachine { get; private set; }

    public AlivePlayerStateMachine(AlivePlayer player)
    {
        Player = player;
        ReusableData = new AlivePlayerStateReusableData();

        MovementStateMachine = new AlivePlayerMovementStateMachine(this);
        CombatStateMachine = new AlivePlayerCombatStateMachine(this);
    }

    public void Update()
    {
        MovementStateMachine.Update();
        CombatStateMachine.Update();
    }

    public void FixedUpdate()
    {
        MovementStateMachine.FixedUpdate();
        CombatStateMachine.FixedUpdate();
    }
}
