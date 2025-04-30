using UnityEngine;

public class ForestScene : MonoBehaviour
{
    private void Awake()
    {
        Managers.SubscribeToInit(() =>
        {
            Managers.Quest.Register(1001);
            Managers.Quest.Register(1002);
        });
    }
}
