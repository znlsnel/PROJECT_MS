using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TimeSystem))]
public class ForestScene : SceneBase
{
    [SerializeField] private GameObject _resultUIPrefab;
    [SerializeField] private int _mapIndex = 1000;
    private PlayableDirector _endingTimeline;
    private ResultUI _resultUI;
    private MapData _mapData;
    private List<GameObject> _fieldItemList = new();
    private List<GameObject> _fieldResourceList = new();

    public static bool isCompleted = false;

    public static Action onCompleted;
    protected override void Awake()
    {
        base.Awake();

        _endingTimeline = gameObject.GetComponent<PlayableDirector>();  

        Managers.SubscribeToInit(InitScene);
    }
 
    void Start()
    {
        Managers.Analytics.SurvivalStart();
        NetworkGameSystem.onGameEnd += ShowResultUI;
    }

    private void InitScene()
    {
        _mapData = Managers.Data.maps.GetByIndex(_mapIndex);
        Quest mainQuest = Managers.Quest.Register(_mapData.MainQuest);
        MyDebug.Log($"퀘스트 추가");
        //Quest subQuest = Managers.Quest.Register(mapData.SubQuest); 

        mainQuest.onCompleted += (q)=>OnCompletedMainQuest();
       // PlaceFieldItem();
       // PlaceFieldResource();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCompletedMainQuest()
    {
        RPC_OnCompletedMainQuest(); 
    }

    [ObserversRpc]
    private void RPC_OnCompletedMainQuest()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.UI.HideSceneUI();

        isCompleted = true;
        onCompleted?.Invoke(); 

        _endingTimeline.Play();
    }


    public void ShowResultUI()
    {
        if (_resultUI == null)
            _resultUI = Instantiate(_resultUIPrefab).GetComponent<ResultUI>();

        Managers.UI.CloseAllPopupUI();
        Managers.Analytics.SurvivalEnding();
        Managers.Analytics.MafiaWinRate(true);

        _resultUI.Setup();
        Managers.UI.ShowPopupUI(_resultUI);

        Managers.Analytics.SurvivalRate(!Managers.Player.IsDead);
    }

    private void PlaceFieldItem()
    {
        for (int i = 0; i < _mapData.FieldItemList.Count; i++)
        {
            int randomIndex = GetRandomIndex(_mapData.FieldItemRatio);
            var go = Managers.Resource.Instantiate(_mapData.FieldItemList[randomIndex].PrefabPath);
            _fieldItemList.Add(go);
        }
    }

    private void PlaceFieldResource()
    {
        for (int i = 0; i < _mapData.FieldResourceList.Count; i++)
        {
            int randomIndex = GetRandomIndex(_mapData.FieldResourceRatio);
            var go = Managers.Resource.Instantiate(_mapData.FieldResourceList[randomIndex].PrefabPath);
            _fieldResourceList.Add(go);
        }
    }


    private int GetRandomIndex(List<int> ratioList)
    {
        int random = Random.Range(0, 100);
 

        return 0;

    }
}
