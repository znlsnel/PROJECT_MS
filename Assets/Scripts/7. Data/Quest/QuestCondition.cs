using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestCondition
{
    // 빈 값이 들어가면 없다고 간주
    [SerializeField] private List<int> preQuestId;
    [SerializeField] private List<int> requiredItemId;  
    // [SerializeField] private int requiredLevel;

    public IReadOnlyList<int> PreQuestId => preQuestId;
    public IReadOnlyList<int> RequiredItemId => requiredItemId;
    // public int RequiredLevel => requiredLevel;

    public QuestCondition(List<int> preQuestId, List<int> requiredItemId)
    {
        this.preQuestId = preQuestId; 
        this.requiredItemId = requiredItemId;
        // this.requiredLevel = requiredLevel;
    }
}
