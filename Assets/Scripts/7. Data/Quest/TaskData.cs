using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TaskData
{
    // "Task Info"
    private int taskId;
    private string taskTitle;


    // "Task Setting"
    private ETaskCategory taskCategory;
    private ETaskActionType actionType;
    private int targetId;


    // "Task Option"
    private int successCount;
    private bool canReceiveReportsDuringCompletion;


    // Properties
    #region Properties
    public int TaskId => taskId;
    public string TaskTitle => taskTitle;
    public ETaskCategory TaskCategory => taskCategory;
    public ETaskActionType ActionType => actionType;
    public int TargetId => targetId;
    public int SuccessCount => successCount;
    public bool CanReceiveReportsDuringCompletion => canReceiveReportsDuringCompletion;
    #endregion

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