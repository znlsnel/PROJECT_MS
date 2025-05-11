using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;

[GoogleSheet.Core.Type.UGS(typeof(EQuestState))]
public enum EQuestState
{
    Inactive,
    Running,
    Complete, 
    Cancel, 
    WaitingForCompletion
}

public class QuestData 
{
    // "Quest Info"
    private int questId;  
    private string title;


    // "Quest Setting"
    private List<TaskData> tasks = new List<TaskData>(); // 몬스터 잡기 or 몬스터한테 맞기 or 인벤토리 열기 등등
    private QuestReward reward; // 보상 -> 아이템 or 골드 or 스킬 or 버프 등등
    private QuestCondition acceptionCondition; // 선행 퀘스트 or 레벨 조건 등등
 

    // "Quest Option"
    private bool useAutoComplete;
    private bool isCancelable;
    private bool isSavable;


    #region Properties
    public IReadOnlyList<TaskData> Tasks => tasks; 
    public QuestReward Reward => reward; 
    public QuestCondition AcceptionConditions => acceptionCondition; 

    public int QuestId => questId; 
    public string Title => title;

    public bool UseAutoComplete => useAutoComplete;
    public bool IsCancelable => isCancelable;
    public bool IsSavable => isSavable; 

    #endregion


    public QuestData(GameData.Quest quest)
    {
        questId = quest.index; 
        title = quest.title;

        foreach (var task in quest.tasks)
        {
            tasks.Add(Managers.Data.questTasks.GetByIndex(task));
        } 

        reward = new QuestReward(quest.rewardItems);
        acceptionCondition = new QuestCondition(quest.requiredQuest, quest.requiredItem);
        
        useAutoComplete = quest.useAutoComplete;
        isCancelable = quest.isCancelable;
        isSavable = quest.isSavable; 
    }

}