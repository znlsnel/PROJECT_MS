using UnityEngine;

public class HealthResource : ResourceStat
{
    [SerializeField] public float defaultHealth;
    [SerializeField] public float defaultMaxHealth;


    public override void OnStartClient()
    {
        Init(defaultMaxHealth, defaultHealth);
    }
}
