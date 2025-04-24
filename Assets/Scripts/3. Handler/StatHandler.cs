using System;
using System.Collections;
using System.Collections.Generic;
using GoogleSheet.Core.Type;
using UnityEngine;
using UnityEngine.UIElements; 






public class StatHandler : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 10;

    private Dictionary<EStatType, Stat> stats;
    private Dictionary<EStatType, ResourceStat> resourceStats;

    private void Awake()
    {
        stats = new Dictionary<EStatType, Stat>();
        resourceStats = new Dictionary<EStatType, ResourceStat>();
        resourceStats[EStatType.Health] = new ResourceStat(health); 

        Managers.Data.SubscribeToDataLoaded(Init);
    } 

    // Stat 데이터 -> 종족, 직업 모두 하나의 시트에서 관리 
    // 변수에 접근하는 방식은 같지만 하나하나 따로 작성해야함
    // private static Dictionary<EStatType, Stat> ConvertToStat(CharacterData.CharacterData data)
    // {
    //     return new Dictionary<EStatType, Stat>
    //     {
    //         {EStatType.Con, new Stat(data.CON)},
    //         {EStatType.Str, new Stat(data.STR)},
    //         {EStatType.Dex, new Stat(data.DEX)},
    //         {EStatType.Int, new Stat(data.INT)},
    //         {EStatType.Wis, new Stat(data.WIS)},
    //         {EStatType.End, new Stat(data.END)},
    //     }; 
    // }

    
    private void Init()
    {
        // var data = Managers.Data.Characters.GetByIndex(Managers.UserData.jobClassId);

        // if (data == null) 
        //     return;
 
        // var newStat = ConvertToStat(data);

        // foreach (var stat in newStat)
        //     stats[stat.Key] = stat.Value; 
    }

    public Stat GetStatData(EStatType statType) => stats[statType]; 
    public ResourceStat GetResourceStatData(EStatType resourceStatType) => resourceStats[resourceStatType]; 

    public float GetStat(EStatType statType) => stats[statType].Value;
    public void BindingActionToStat(EStatType statType, Action<float> action)
    {
        stats[statType].OnValueChanged += action; 
    }
 
    public void TakeDamage(float damage)
    {
        float prevHealth = resourceStats[EStatType.Health].Current;
        resourceStats[(int)EStatType.Health].Subtract(damage); 
        Debug.Log($"[{this.name}]  Health: {prevHealth} -> {resourceStats[(int)EStatType.Health].Current}");
    }

    public float GetAttackValue()
    {
        // TODO
        // 각 스탯들 조합해서 공격력 가져오기
        return 0.1f;
    }

    public float GetSpeed()
    {
        // TODO
        return GetStat(EStatType.Speed);
    }
}
 