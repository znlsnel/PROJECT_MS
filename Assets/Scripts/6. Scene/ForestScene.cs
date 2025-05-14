using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestScene : MonoBehaviour
{
    [SerializeField] private GameObject _resultUIPrefab;

    private ResultUI _resultUI;

    private void Awake()
    {
        Managers.SubscribeToInit(InitScene);
        _resultUI = Instantiate(_resultUIPrefab).GetComponent<ResultUI>();
        _resultUI.Hide();
    }

    private void InitScene()
    {
        Quest mainQuest =Managers.Quest.Register(1003);
        Managers.Quest.Register(1001);
        Managers.Quest.Register(1002);


        mainQuest.onCompleted += OnCompletedMainQuest;
    }


    private void OnCompletedMainQuest(Quest quest)
    {
        Managers.UI.CloseAllPopupUI();
        Managers.UI.ShowPopupUI(_resultUI);
    }
}
