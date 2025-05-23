using System;
using System.Runtime.CompilerServices;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(InteractionHandler))]
public class AlivePlayer : NetworkBehaviour, IDamageable
{
    #region Stats
    [field: SerializeField] public HealthResource Health {get; private set;}
    [field: SerializeField] public StaminaResource Stamina {get; private set;}

   // private readonly SyncVar<Inventory> Inventory = new SyncVar<Inventory>(new Inventory());
    public Inventory Inventory {get; private set;} = new Inventory();
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

        onDead += OnDead;
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

        NetworkGameSystem.Instance.IsGameStarted.OnChange += OnGameStarted;
    }

    private void OnGameStarted(bool prev, bool next, bool asServer)
    {
        if(!next)
        {
            Debug.Log("OnGameStarted");
            NetworkCommandSystem.Instance.RequestDespawnPlayer(NetworkObject);
        }
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

        if(Health.Current.Value <= 0)
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
        OnTakeDamage();
    }

    [ObserversRpc]
    public void OnTakeDamage()
    {
        onDamaged?.Invoke(); 

        if(Health.Current.Value <= 0)
        {
            onDead?.Invoke();
        }
    }

    public void ChangeWeapon(WeaponController weapon)
    {
        if(!IsOwner)
            return;

        WeaponHandler = weapon;

        int holdAnimationIndex = Managers.Data.Animation.GetIndex(WeaponHandler.holdAnimation);
        int attackAnimationIndex = Managers.Data.Animation.GetIndex(WeaponHandler.attackAnimation);
        float speed = WeaponHandler.attackAnimationSpeed;
        bool isHolding = WeaponHandler != null;

        //OnChangeWeapon(holdAnimationIndex, attackAnimationIndex, speed, isHolding);

        ServerRpcOnChangeWeapon(holdAnimationIndex, attackAnimationIndex, speed, isHolding);
    }

    private void OnChangeWeapon(int holdAnimationIndex, int attackAnimationIndex, float speed, bool isHolding)
    {
        overrideController["Holding"] = Managers.Data.Animation.GetByIndex(holdAnimationIndex);
        overrideController["Attack"] = Managers.Data.Animation.GetByIndex(attackAnimationIndex);
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
        Health.Add(amount);
    }

    public void OnDead()
    {
        NetworkGameSystem.Instance.OnPlayerDead();
    }
}
