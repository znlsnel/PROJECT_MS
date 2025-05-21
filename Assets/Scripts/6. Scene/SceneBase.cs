using UnityEngine;

public class SceneBase : MonoBehaviour
{
    public TimeSystem TimeSystem {get; private set;}


    protected virtual void Awake()
    {
        TimeSystem = gameObject.GetOrAddComponent<TimeSystem>();

        Managers.SubscribeToInit(()=>{
            Managers.SetScene(this);
        });
    }
}
