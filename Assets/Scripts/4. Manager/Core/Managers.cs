
using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : Singleton<Managers>
{ 
    #region Manager Class
    [SerializeField] private DataManager data;
    [SerializeField] private InputManager input;
    [SerializeField] private ResourceManager resource;
    [SerializeField] private UIManager ui;
    [SerializeField] private PoolManager pool;
    [SerializeField] private NetworkManagerEx network;
    [SerializeField] private SteamManagerEx steam;
    [SerializeField] private QuestManager quest;
    [SerializeField] private AnalyticsManager analytics;
    [SerializeField] private SoundManager sound = new SoundManager(); 
 
    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static SoundManager Sound => Instance.sound; 
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool;
    public static NetworkManagerEx Network => Instance.network;
    public static QuestManager Quest => Instance.quest;
    public static SteamManagerEx Steam => Instance.steam;
    public static AnalyticsManager Analytics => Instance.analytics;

    #endregion
    
    private AlivePlayer player;
    public static AlivePlayer Player => Instance.player;
    public static event Action<AlivePlayer> onChangePlayer;

    public static event Action onInit;
    private static bool isInit = false;

    public static SceneBase scene {get; private set;}

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
        // if (isInit)
        //     return;

        analytics.Init();
        Network.Init();
        Resource.Init(); 
        Data.Init();
        quest.Init();
        Input.Init();
 
        Sound.Init();  
        UI.Init();
        Pool.Init();
        Steam.Init();

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

    public static void SetScene(SceneBase s)
    {
        scene = s;
    }

}

public enum NetworkType
{
    TCP_UDP,
    Steam
}