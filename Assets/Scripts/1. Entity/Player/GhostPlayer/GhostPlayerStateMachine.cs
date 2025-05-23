using UnityEngine;


public class GhostPlayerStateMachine
{
    public GhostPlayer Player { get; private set; }
    public GhostPlayerStateReusableData ReusableData { get; private set; }

    public GhostPlayerMovementStateMachine MovementStateMachine { get; private set; }
    
    public GhostPlayerStateMachine(GhostPlayer player)
    {
        Player = player;
        ReusableData = new GhostPlayerStateReusableData();

        MovementStateMachine = new GhostPlayerMovementStateMachine(this);
        MovementStateMachine.Init();
    }

    public void Update()
    {
        MovementStateMachine.Update();
    }

    public void FixedUpdate()
    {
        MovementStateMachine.FixedUpdate();
    }
}

