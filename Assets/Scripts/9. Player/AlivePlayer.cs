using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(InteractionHandler))]
public class AlivePlayer : MonoBehaviour
{
    #region Stats
    protected ResourceStat health;
    protected ResourceStat hungerPoint;
    protected ResourceStat waterPoint;
    protected ResourceStat stamina;
    protected ResourceStat temperature;
    protected ResourceStat sanity;

    public ResourceStat Health => health;
    public ResourceStat HungerPoint => hungerPoint;
    public ResourceStat WaterPoint => waterPoint;
    public ResourceStat Stamina => stamina;
    public ResourceStat Temperature => temperature;
    public ResourceStat Sanity => sanity;
    #endregion

    public PlayerController PlayerController { get; private set; }
    public InteractionHandler InteractionHandler { get; private set; }
    private AlivePlayerStateMachine stateMachine;

    public void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        InteractionHandler = GetComponent<InteractionHandler>();
    }

    public void Start()
    {
        health = new ResourceStat(100);
        hungerPoint = new ResourceStat(100);
        waterPoint = new ResourceStat(100);
        stamina = new ResourceStat(100);
        temperature = new ResourceStat(100);

        stateMachine = new AlivePlayerStateMachine(this);
    }
}
