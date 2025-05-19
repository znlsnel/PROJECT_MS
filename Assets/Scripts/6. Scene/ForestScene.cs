using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class ForestScene : MonoBehaviour
{
    [SerializeField] private GameObject _resultUIPrefab;
    [SerializeField] private int _mapIndex = 1000;
    private PlayableDirector _endingTimeline;

    private ResultUI _resultUI;


    private MapData _mapData;
    private List<GameObject> _fieldItemList = new();
    private List<GameObject> _fieldResourceList = new();

    private void Awake()
    {
        _endingTimeline = GetComponent<PlayableDirector>(); 
        Managers.SubscribeToInit(InitScene);
        _resultUI = Instantiate(_resultUIPrefab).GetComponent<ResultUI>();
        _resultUI.Hide();
    }

    private void InitScene()
    {
        _mapData = Managers.Data.maps.GetByIndex(_mapIndex);
        Quest mainQuest = Managers.Quest.Register(_mapData.MainQuest);
        //Quest subQuest = Managers.Quest.Register(mapData.SubQuest);

        mainQuest.onCompleted += OnCompletedMainQuest;
       // PlaceFieldItem();
       // PlaceFieldResource();
    }


    private void OnCompletedMainQuest(Quest quest)
    {
        // TODO 타임라인 플레이
        Managers.UI.CloseAllPopupUI();
        Managers.UI.HideSceneUI();

        _endingTimeline.Play();
    }

    public void ShowResultUI()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.UI.ShowPopupUI(_resultUI);
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
