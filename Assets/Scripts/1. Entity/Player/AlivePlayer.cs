using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(InteractionHandler))]
public class AlivePlayer : MonoBehaviour
{
    #region Stats
    protected ResourceStat health;
    protected ResourceStat hungerPoint;
    protected ResourceStat waterPoint;
    protected ResourceStat stamina;
    protected ResourceStat temperature;
    protected ResourceStat sanity;

    public ResourceStat Health => health;
    public ResourceStat HungerPoint => hungerPoint;
    public ResourceStat WaterPoint => waterPoint;
    public ResourceStat Stamina => stamina;
    public ResourceStat Temperature => temperature;
    public ResourceStat Sanity => sanity;
    #endregion

    public string HoldingItem { get; private set; }
    
    public InteractionHandler InteractionHandler { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    [field: SerializeField] public AnimatorOverrideController overrideController { get; private set; }
    [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
    [field: SerializeField] public AlivePlayerSO AlivePlayerSO { get; private set; }
    [field: SerializeField] public AlivePlayerAnimationData AnimationData { get; private set; }
    private AlivePlayerStateMachine stateMachine;

    public void Awake()
    {
        InteractionHandler = GetComponent<InteractionHandler>();
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        AnimationData = new AlivePlayerAnimationData();

        overrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = overrideController;
    }

    public void Start()
    {
        health = new ResourceStat(100);
        hungerPoint = new ResourceStat(100);
        waterPoint = new ResourceStat(100);
        stamina = new ResourceStat(100);
        temperature = new ResourceStat(100);

        stateMachine = new AlivePlayerStateMachine(this);
    }

    public void Update()
    {
        stateMachine.Update();
    }

    public void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
