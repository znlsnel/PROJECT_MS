using UnityEngine;

public class SceneUI : UIBase
{

    protected override void Awake()
    {
        base.Awake();
        Managers.UI.RegisterSceneUI(this);
    }

}
