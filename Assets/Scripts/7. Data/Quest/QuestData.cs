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

public class QuestData : ScriptableObject 
{
    [Header("Quest Info")]
    [SerializeField] private int questId;  
    [SerializeField] private Sprite icon;
    [SerializeField] private string questName;
    [SerializeField, TextArea] private string questDescription;


    [Header("Quest Setting")]
    [SerializeField] private List<TaskData> tasks; // 몬스터 잡기 or 몬스터한테 맞기 or 인벤토리 열기 등등
    [SerializeField] private QuestReward reward; // 보상 -> 아이템 or 골드 or 스킬 or 버프 등등
    [SerializeField] private QuestCondition acceptionCondition; // 선행 퀘스트 or 레벨 조건 등등
 

    [Header("Option")]
    [SerializeField] private bool useAutoComplete;
    [SerializeField] private bool isCancelable;
    [SerializeField] private bool isSavable;


    #region Properties
    
 
    public IReadOnlyList<TaskData> Tasks => tasks; 
    public QuestReward Rewards => reward; 
    public QuestCondition AcceptionConditions => acceptionCondition; 

    public int QuestId => questId; 
    public Sprite Icon => icon;
    public string QuestName => questName;
    public string QuestDescription => questDescription;

    public bool UseAutoComplete => useAutoComplete;
    public bool IsCancelable => isCancelable;
    public bool IsSavable => isSavable; 

    #endregion

}