using UnityEngine;

public class StaminaResource : ResourceStat
{
    [SerializeField] private float defaultStamina;
    [SerializeField] private float defaultMaxStamina;

    public override void OnStartClient()
    {
        Init(defaultMaxStamina, defaultStamina);
    }
}
