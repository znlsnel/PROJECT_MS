using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestScene : MonoBehaviour
{
    private void Awake()
    {
        Managers.SubscribeToInit(InitScene);
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
        SceneManager.LoadScene("Lobby");
    }
}
