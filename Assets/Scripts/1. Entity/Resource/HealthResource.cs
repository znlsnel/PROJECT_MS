using UnityEngine;

public class HealthResource : ResourceStat
{
    [SerializeField] private float defaultHealth;
    [SerializeField] private float defaultMaxHealth;

    public override void OnStartClient()
    {
        Init(defaultMaxHealth, defaultHealth);
    }
}
