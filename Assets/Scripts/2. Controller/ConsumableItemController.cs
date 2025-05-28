using FishNet.Component.Observing;
using UnityEngine;

public class ConsumableItemController : ItemController
{
    private static GameObject _useEffect;

    private readonly static string fxPath = "FX/ConsumableFX.prefab";

    private void Start()
    {
        if ( _useEffect == null)
        {
            _useEffect = Resources.Load<GameObject>(fxPath);
        } 
    }

    public override void OnAction()
    {
        // 체력, 배고픔, 물, 스태미나, 온도, 정신력 회복
        
        Owner.Health.Add(itemData.Heal);
        Owner.Stamina.Add(itemData.RestoreStamina);

        itemSlot.AddStack(itemData, -1);

        GameObject effect = Managers.Pool.Get(fxPath);
        effect.transform.position = Owner.transform.position; 

        Managers.Pool.Release(effect, 2f);   
        
    }
}
