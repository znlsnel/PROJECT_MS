using FishNet.Object;
using Unity.Cinemachine;
using UnityEngine;

public class GhostPlayer : NetworkBehaviour
{
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }

    [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
    [field: SerializeField] public GhostPlayerSO GhostPlayerSO { get; private set; }
    [field: SerializeField] public GhostPlayerAnimationData AnimationData { get; private set; }
    
    private GhostPlayerStateMachine stateMachine;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        AnimationData = new GhostPlayerAnimationData();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(!IsOwner)
            return;

        CinemachineCamera = GameObject.FindWithTag("MainCinemachineCamera").GetComponent<CinemachineCamera>();
        CinemachineCamera.Follow = transform;

        stateMachine = new GhostPlayerStateMachine(this);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public void Update()
    {
        if(!IsOwner)
            return;

        stateMachine.Update();
    }

    public void FixedUpdate()
    {
        if(!IsOwner)
            return;

        stateMachine.FixedUpdate();
    }
}
