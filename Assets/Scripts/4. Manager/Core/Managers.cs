
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;



public class Managers : Singleton<Managers>
{ 
    [field: SerializeField] private DataManager data  = new DataManager();
    [field: SerializeField] private InputManager input = new InputManager();
    [field: SerializeField] private ResourceManager resource = new ResourceManager();
    [field: SerializeField] private SoundManager sound = new SoundManager();
    [field: SerializeField] private UIManager ui = new UIManager();
    [field: SerializeField] private PoolManager pool = new PoolManager();
    [field: SerializeField] private SceneManagerEx scene = new SceneManagerEx();
    [field: SerializeField] private QuestManager quest = new QuestManager();
    [field: SerializeField] private SteamManagerEx steam = new SteamManagerEx();
    private UserData userData = new UserData();

    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static SoundManager Sound => Instance.sound; 
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool;
    public static SceneManagerEx Scene => Instance.scene;
    public static QuestManager Quest => Instance.quest;
    public static SteamManagerEx Steam => Instance.steam; 
    public static UserData UserData => Instance.userData;

    private AlivePlayer player;
    public static AlivePlayer Player => Instance.player;
    public static event Action<AlivePlayer> onChangePlayer;

    public Action onInit;
    private bool isInit = false;

    protected override void Awake()
    {
        base.Awake();

        if (isDestroy)
            return; 

        Init();
    }

    private void Init() 
    {
        Debug.Log("Managers Init");
        Data.Init();
        quest.Init();
        Input.Init();
        Resource.Init(); 
        Sound.Init(); 
        UI.Init();
        Pool.Init();
        Scene?.Init();
        userData.Init(); 

        Steam.Init(); 

        isInit = true;
        onInit?.Invoke(); 
        onInit = null;
    }

    public static void SubscribeToInit(Action callback)
    {
        if (Instance.isInit)
            callback?.Invoke();
        else
            Instance.onInit += callback;  
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
