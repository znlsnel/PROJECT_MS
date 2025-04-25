using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETaskState
{
    Inactive,
    Running,
    Complete
}




public class TaskData : ScriptableObject
{
    [Header("Task Info")]
    [SerializeField] private int taskId;
    [SerializeField] private string taskTitle;


    [Header("Task Setting")]
    [SerializeField] private ETaskCategory taskCategory;
    [SerializeField] private ETaskActionType actionType;
    [SerializeField] private int targetId;


    [Header("Task Option")]
    [SerializeField] private int successCount;
    [SerializeField] private bool canReceiveReportsDuringCompletion;


    // Properties
    public int TaskId => taskId;
    public string TaskTitle => taskTitle;
    public ETaskCategory TaskCategory => taskCategory;
    public ETaskActionType ActionType => actionType;
    public int TargetId => targetId;
    public int SuccessCount => successCount;
    public bool CanReceiveReportsDuringCompletion => canReceiveReportsDuringCompletion;

  
    public TaskData(GameData.QuestTask task)
    {
        taskId = task.index;
        taskTitle = task.title;  
        taskCategory = task.taskType;
        actionType = task.actionType;
        targetId = task.targeId;
        successCount = task.successCount;
        canReceiveReportsDuringCompletion = task.canReceiveReportsDuringCompletion;
    }
} 