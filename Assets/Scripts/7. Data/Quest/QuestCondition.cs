using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class QuestCondition
{
    // 빈 값이 들어가면 없다고 간주
    private List<int> preQuestId = new List<int>();
    private List<int> requiredItemId = new List<int>();   
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
