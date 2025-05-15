using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TaskData
{

    public int TaskId {get; private set;}
    public int LinkedTaskId {get; private set;}
    public string TaskTitle {get; private set;}
    public ETaskCategory TaskCategory {get; private set;}
    public ETaskActionType ActionType {get; private set;}
    public int TargetId {get; private set;}
    public int SuccessCount {get; private set;}
    public bool CanReceiveReportsDuringCompletion {get; private set;}


    public TaskData(GameData.QuestTask task)
    {
        TaskId = task.index;
        LinkedTaskId = task.linkedTask;
        TaskTitle = task.title;    
        TaskCategory = task.taskType;
        ActionType = task.actionType;
        TargetId = task.targeId;
        SuccessCount = task.successCount;
        CanReceiveReportsDuringCompletion = task.canReceiveReportsDuringCompletion;
    }
}