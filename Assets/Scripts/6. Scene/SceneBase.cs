using FishNet.Object;
using UnityEngine;

public class SceneBase : NetworkBehaviour 
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
