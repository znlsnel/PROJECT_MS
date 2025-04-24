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
    [SerializeField] private int requiredLevel;
}
