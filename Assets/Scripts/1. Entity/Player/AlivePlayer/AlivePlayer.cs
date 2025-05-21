using System;
using System.Runtime.CompilerServices;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(InteractionHandler))]
public class AlivePlayer : NetworkBehaviour, IDamageable
{
    #region Stats
    protected readonly SyncVar<ResourceStat> health = new SyncVar<ResourceStat>(new ResourceStat(100));
    protected readonly SyncVar<ResourceStat> hungerPoint = new SyncVar<ResourceStat>(new ResourceStat(100));
    protected readonly SyncVar<ResourceStat> waterPoint = new SyncVar<ResourceStat>(new ResourceStat(100));
    protected readonly SyncVar<ResourceStat> stamina = new SyncVar<ResourceStat>(new ResourceStat(100));
    protected readonly SyncVar<ResourceStat> temperature = new SyncVar<ResourceStat>(new ResourceStat(100));
    protected readonly SyncVar<ResourceStat> sanity = new SyncVar<ResourceStat>(new ResourceStat(100));
   // private readonly SyncVar<Inventory> Inventory = new SyncVar<Inventory>(new Inventory());
    public Inventory Inventory {get; private set;} = new Inventory();

    public ResourceStat Health => health.Value;
    public ResourceStat HungerPoint => hungerPoint.Value;
    public ResourceStat WaterPoint => waterPoint.Value;
    public ResourceStat Stamina => stamina.Value;
    public ResourceStat Temperature => temperature.Value;
    public ResourceStat Sanity => sanity.Value;
    #endregion

    public InteractionHandler InteractionHandler { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    [field: SerializeField] public AnimatorOverrideController overrideController { get; private set; }
    [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
    [field: SerializeField] public AlivePlayerSO AlivePlayerSO { get; private set; }
    [field: SerializeField] public AlivePlayerAnimationData AnimationData { get; private set; }
    [field: SerializeField] public WeaponController WeaponHandler { get; private set; }
    private AlivePlayerStateMachine stateMachine;

    public QuickSlotHandler QuickSlotHandler {get; private set;}
    public ItemHandler ItemHandler {get; private set;}
    public PlacementHandler PlacementHandler {get; private set;}
    public EquipHandler EquipHandler {get; private set;}


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

        EquipHandler = gameObject.GetOrAddComponent<EquipHandler>();
        EquipHandler.Setup(Inventory);

        QuickSlotHandler = gameObject.GetOrAddComponent<QuickSlotHandler>();
        QuickSlotHandler.Setup(Inventory);

        ItemHandler = gameObject.GetOrAddComponent<ItemHandler>();
        ItemHandler.Setup(QuickSlotHandler); 

        PlacementHandler = gameObject.GetOrAddComponent<PlacementHandler>();
        PlacementHandler.Setup(QuickSlotHandler);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(!IsOwner)
            return;

        Init();

        Managers.Instance.SetPlayer(this);
        CinemachineCamera = GameObject.FindWithTag("MainCinemachineCamera").GetComponent<CinemachineCamera>();
        CinemachineCamera.Follow = transform;

        stateMachine = new AlivePlayerStateMachine(this);
    }

    [ServerRpc]
    private void Init()
    {
        
    }

    public void Update()
    {
        if(!IsOwner)
            return;

        stateMachine.Update();

       // hungerPoint.Subtract(Time.deltaTime * 10f); // 추후 float 값 수정
       // waterPoint.Subtract(Time.deltaTime * 0.5f);

        if(hungerPoint.Value.Current <= 0)
        {
            Health.Subtract(Time.deltaTime * 5f);
        }

        if(Input.GetKeyDown(KeyCode.P)) // 테스트 코드
        {
            RestoreHunger(30f);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10, null); 
        }
    }


    public void FixedUpdate()
    {
        if(!IsOwner)
            return;

        stateMachine.FixedUpdate();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(float damage, GameObject attacker)
    {
        Health.Subtract(damage);
        onDamaged?.Invoke(); 

        if(Health.Current <= 0)
        {
            onDead?.Invoke();
        }
    }

    public void ChangeWeapon(WeaponController weapon)
    {
        if(!IsOwner)
            return;

        WeaponHandler = weapon;
        
        int holdAnimationIndex = Managers.Data.animation.GetIndex(WeaponHandler.holdAnimation);
        int attackAnimationIndex = Managers.Data.animation.GetIndex(WeaponHandler.attackAnimation);
        float speed = WeaponHandler.attackAnimationSpeed;
        bool isHolding = WeaponHandler != null;

        OnChangeWeapon(holdAnimationIndex, attackAnimationIndex, speed, isHolding);

        ServerRpcOnChangeWeapon(holdAnimationIndex, attackAnimationIndex, speed, isHolding);
    }

    private void OnChangeWeapon(int holdAnimationIndex, int attackAnimationIndex, float speed, bool isHolding)
    {
        overrideController["Holding"] = Managers.Data.animation.GetByIndex(holdAnimationIndex);
        overrideController["Attack"] = Managers.Data.animation.GetByIndex(attackAnimationIndex);
        Animator.SetFloat("AttackSpeed", speed);
        Animator.SetBool(AnimationData.HoldingParameterHash, isHolding);
    }

    [ServerRpc]
    private void ServerRpcOnChangeWeapon(int holdAnimationIndex, int attackAnimationIndex, float speed, bool isHolding)
    {
        ObserverRpcChangeWeapon(holdAnimationIndex, attackAnimationIndex, speed, isHolding);
    }

    [ObserversRpc]
    private void ObserverRpcChangeWeapon(int holdAnimationIndex, int attackAnimationIndex, float speed, bool isHolding)
    {
        OnChangeWeapon(holdAnimationIndex, attackAnimationIndex, speed, isHolding);
    }

    [ServerRpc]
    public void RestoreHunger(float amount) // 음식물 섭취
    {
        hungerPoint.Value.Add(amount);
    }

    [ServerRpc]
    public void RestoreWater(float amount) // 물 섭취
    {
        waterPoint.Value.Add(amount);
    }
}
