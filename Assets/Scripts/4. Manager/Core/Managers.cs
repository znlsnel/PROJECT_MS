
using System;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : Singleton<Managers>
{ 
    [field: SerializeField] private DataManager data  = new DataManager();
    [field: SerializeField] private InputManager input = new InputManager();
    [field: SerializeField] private ResourceManager resource = new ResourceManager();
    [field: SerializeField] private SoundManager sound = new SoundManager();
    [field: SerializeField] private UIManager ui = new UIManager();
    [field: SerializeField] private PoolManager pool = new PoolManager();
    [field: SerializeField] private NetworkManagerEx network = new NetworkManagerEx();
    [field: SerializeField] private SteamManagerEx steam = new SteamManagerEx();
    [field: SerializeField] private QuestManager quest = new QuestManager();
    private UserData userData = new UserData();

    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static SoundManager Sound => Instance.sound; 
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool;
    public static NetworkManagerEx Network => Instance.network;
    public static QuestManager Quest => Instance.quest;
    public static SteamManagerEx Steam => Instance.steam;
    public static UserData UserData => Instance.userData;

    private AlivePlayer player;
    public static AlivePlayer Player => Instance.player;
    public static event Action<AlivePlayer> onChangePlayer;

    public static event Action onInit;
    private static bool isInit = false;

    protected override void Awake()
    {
        base.Awake();

        if (isDestroy)
            return; 

        Init();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        network.OnValidate();
    }
#endif

    private void Init() 
    {
        Debug.Log("Managers Init");
        Network.Init();
        Data.Init();
        quest.Init();
        Input.Init();
        Resource.Init(); 
        Sound.Init(); 
        UI.Init();
        Pool.Init();
        Steam.Init();
        userData.Init(); 

        isInit = true;
        onInit?.Invoke(); 
        onInit = null;
    }

    public static void SubscribeToInit(Action callback)
    {
        if (isInit)
            callback?.Invoke();
        else
            onInit += callback;  
    }

    public void Update()
    {
        if(Network.Type == NetworkType.Steam)
            steam.Update();
    }

    public static void Clear()
    {

    }

    public void SetPlayer(AlivePlayer player)
    {
        this.player = player;
        onChangePlayer?.Invoke(player);
    }
}

public enum NetworkType
{
    TCP_UDP,
    Steam
}