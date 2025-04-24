
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;


public abstract class Manager
{
    public Action onInit;
    private bool isInit = false;

    public void SubscribeToInit(Action callback)
    {
        if (isInit)
            callback?.Invoke();
        else
            onInit += callback;
    }

    public void Setup()
    {
        if (isInit)
            return;

        Init();
        isInit = true; 
        onInit?.Invoke();
        onInit = null;
    }

    protected abstract void Init();
    public abstract void Clear();
}

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

    protected override void Awake()
    {
        base.Awake();



        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        Data.Setup();
        quest.Setup();
        Input.Setup();
        Resource.Setup(); 
        Sound.Setup(); 
        UI.Setup();
        Pool.Setup();
        Scene?.Init();


        Steam.Setup(); 
    }

    public static void Clear()
    {

    }

}
