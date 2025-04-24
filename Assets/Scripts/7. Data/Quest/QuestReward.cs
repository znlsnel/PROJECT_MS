using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestReward
{
    // 빈 값이 들어가면 없다고 간주
    [SerializeField] private List<int> rewardItemId;
    // [SerializeField] private int rewardGold;
    // [SerializeField] private int rewardExp;

    public IReadOnlyList<int> RewardItemId => rewardItemId;
    // public int RewardGold => rewardGold;
    // public int RewardExp => rewardExp;

    public QuestReward(List<int> rewardItemId)
    {
        this.rewardItemId = rewardItemId; 
        // this.rewardGold = rewardGold;
        // this.rewardExp = rewardExp;
    }
}
 