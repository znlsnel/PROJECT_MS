using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTask
{
    public TaskData taskData {get; private set;}
    public ETaskState State { get; private set; } = ETaskState.Inactive;
    public int progress {get; private set;}
 
    public event Action<QuestTask, ETaskState, ETaskState> onStateChanged;
    public event Action<QuestTask, bool> onSuccessChanged;
    public event Action<QuestTask, int, int> onProgressChanged;
    
   

    public void Initialize(TaskData taskDataSO, ETaskState state = ETaskState.Inactive, int progress = 0)
    {
        this.taskData = taskDataSO;
        State = state;
        this.progress = progress;  
    } 

    public bool IsMatch(ETaskCategory category, int targetId)
    {
        return taskData.TaskCategory == category && taskData.TargetId == targetId;
    }
  
    public void ReceiveReport(int successCount)
    {
        if (State == ETaskState.Complete && taskData.CanReceiveReportsDuringCompletion == false) 
            return;

        progress = TaskAction.Run(taskData.ActionType, progress, successCount);
        progress = Mathf.Clamp(progress, 0, taskData.SuccessCount); 
        Debug.Log($"[{taskData.TaskName}] 진행도 : {progress} / {taskData.SuccessCount}");  

        ETaskState prevState = State;
        if (progress >= taskData.SuccessCount)
        {
            State = ETaskState.Complete;
            if (prevState != ETaskState.Complete){
                onSuccessChanged?.Invoke(this, true);   
            }
        }
        else
        {
            State = ETaskState.Running;
            if (prevState == ETaskState.Complete)
                onSuccessChanged?.Invoke(this, false);   
        }

        onStateChanged?.Invoke(this, State, prevState);   
        onProgressChanged?.Invoke(this, progress, taskData.SuccessCount); 
    }

    public void Register()
    { 
        State = ETaskState.Running;
    } 
} 