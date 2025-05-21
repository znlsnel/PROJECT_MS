using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;
using UnityEngine;

public class TimeSystem : NetworkBehaviour
{
    [Header("Start Time")]
    [SerializeField, Range(0, 23)] private int startHour;
    [SerializeField, Range(0, 59)] private int startMinute; 

    [Header("End Time")]
    [SerializeField, Range(2, 10)] private int endDay;
    [SerializeField, Range(0, 23)] private int endHour;
    [SerializeField, Range(0, 59)] private int endMinute;

    [Header("24시간이 지나는 현실 시간 (분)")]
    [SerializeField, Range(0.1f, 10f)] private float timeScale;

    [Header("DayNightCycle")]
    [SerializeField, Range(6, 10)] private int dayStartHour = 6;
    [SerializeField, Range(0, 59)] private int dayStartMinute = 0;
    [SerializeField, Range(18, 23)] private int dayEndHour = 18;
    [SerializeField, Range(0, 59)] private int dayEndMinute = 0;
    


    private readonly SyncVar<int> currentDay = new SyncVar<int>(0);
    private readonly SyncVar<int> currentHour = new SyncVar<int>(0);
    private readonly SyncVar<int> currentMinute = new SyncVar<int>(0);
    private readonly SyncVar<bool> isEndTime = new SyncVar<bool>(false);

    public event Action<int, int, int> onChangedTime;
    public event Action<int> onChangedDay;
    public event Action onEndTime;

    public float dayStartTime {get; private set;} 
    public float dayEndTime {get; private set;}
        public float startTime {get; private set;} 
    public float fullDayLength {get; private set;} 

    private void Awake()
    {
        dayStartTime = (dayStartHour * 60 + dayStartMinute) / 1440f;
        dayEndTime = (dayEndHour * 60 + dayEndMinute) / 1440f;
        startTime = (startHour * 60 + startMinute) / 1440f;
        fullDayLength = timeScale * 60f;
    }

    public override void OnStartServer()
    {
        if (!IsServerStarted) return;
 
        currentDay.Value = 1;
        currentHour.Value = startHour;
        currentMinute.Value = startMinute;

        float scale = (60f / 60f / 24f) * timeScale;  

       InvokeRepeating(nameof(UpdateTimeOnServer), 0f, scale);   
  
    }
 
    public void Start()
    {
        currentMinute.OnChange += (p, n, s)=>UpdateTime(currentDay.Value, currentHour.Value, currentMinute.Value);
    }

    private void UpdateTimeOnServer()
    {
        if (isEndTime.Value)
            return;

        currentMinute.Value += 1;
        if (currentMinute.Value >= 60)
        {
            currentMinute.Value = 0;
            currentHour.Value += 1;
        }
        if (currentHour.Value >= 24)
        {
            currentHour.Value = 0;
            currentDay.Value += 1;
        }
    }
    

    private void UpdateTime(int d, int h, int m)
    {
        onChangedTime?.Invoke(d, h, m);
        
        if (currentDay.Value < d)
            onChangedDay?.Invoke(d); 

        currentDay.Value = d;
        currentHour.Value = h;
        currentMinute.Value = m;

        if (currentDay.Value >= endDay && currentHour.Value >= endHour && currentMinute.Value >= endMinute)
        {
            isEndTime.Value = true;
            onEndTime?.Invoke();
        }
    }
}
