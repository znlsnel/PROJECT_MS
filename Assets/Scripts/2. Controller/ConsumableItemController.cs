using FishNet.Component.Observing;
using UnityEngine;

public class ConsumableItemController : ItemController
{
    private static GameObject _waterParticle;
    private static GameObject _foodParticle;
    private static GameObject _healParticle;
    private static GameObject _sanityParticle;
    private static GameObject _staminaParticle;
    private static GameObject _temperatureParticle;

    public override void OnAction()
    {
        // 체력, 배고픔, 물, 스태미나, 온도, 정신력 회복
        
        _owner.Health.Add(itemData.Heal);
        _owner.HungerPoint.Add(itemData.RestoreHunger);
        _owner.WaterPoint.Add(itemData.RestoreWater);
        _owner.Stamina.Add(itemData.RestoreStamina);
        _owner.Temperature.Add(itemData.RestoreTemperature);
        _owner.Sanity.Add(itemData.RestoreSanity);

        itemSlot.AddStack(itemData, -1);
    }
}
