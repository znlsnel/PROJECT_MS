using System;
using FishNet.Object;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(InteractionHandler))]
public class AlivePlayer : NetworkBehaviour, IDamageable
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

    public InteractionHandler InteractionHandler { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    [field: SerializeField] public AnimatorOverrideController overrideController { get; private set; }
    [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
    [field: SerializeField] public AlivePlayerSO AlivePlayerSO { get; private set; }
    [field: SerializeField] public AlivePlayerAnimationData AnimationData { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
    private AlivePlayerStateMachine stateMachine;

    public event Action onDead;
    public event Action onDamaged;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        InteractionHandler = GetComponent<InteractionHandler>();
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        AnimationData = new AlivePlayerAnimationData();

        overrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = overrideController;

        health = new ResourceStat(100);
        hungerPoint = new ResourceStat(100);
        waterPoint = new ResourceStat(100);
        stamina = new ResourceStat(100);
        temperature = new ResourceStat(100);

        stateMachine = new AlivePlayerStateMachine(this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(!IsOwner)
            return;

        Managers.Instance.SetPlayer(this);
        CinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
        CinemachineCamera.Follow = transform;
    }

    public void Update()
    {
        if(!IsOwner)
            return;

        stateMachine.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void FixedUpdate()
    {
        if(!IsOwner)
            return;

        stateMachine.FixedUpdate();
    }

    public void TakeDamage(float damage)
    {
        Health.Subtract(damage);
        onDamaged?.Invoke();

        if(Health.Current <= 0)
        {
            onDead?.Invoke();
        }
    }
    
    public void ChangeWeapon(WeaponHandler weapon)
    {
        WeaponHandler = weapon;
        overrideController["Holding"] = WeaponHandler.holdAnimation;
        Animator.SetBool(AnimationData.HoldingParameterHash, WeaponHandler != null);
    }
}
