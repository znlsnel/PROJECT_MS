using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTask
{
    public TaskData taskDataSO {get; private set;}
    public ETaskState State { get; private set; } = ETaskState.Inactive;
    public int progress {get; private set;}
 
    public event Action<QuestTask, ETaskState, ETaskState> onStateChanged;
    public event Action<QuestTask, bool> onSuccessChanged;



    public void Initialize(TaskData taskDataSO, ETaskState state = ETaskState.Inactive, int progress = 0)
    {
        this.taskDataSO = taskDataSO;
        State = state;
        this.progress = progress;  
    } 

    public bool IsMatch(ETaskCategory category, int targetId)
    {
        return taskDataSO.TaskCategory == category && taskDataSO.TargetId == targetId;
    }
  
    public void ReceiveReport(int successCount)
    {
        if (State == ETaskState.Complete && taskDataSO.CanReceiveReportsDuringCompletion == false) 
            return;

        progress = TaskAction.Run(taskDataSO.ActionType, progress, successCount);
        progress = Mathf.Clamp(progress, 0, taskDataSO.SuccessCount); 
        Debug.Log($"[{taskDataSO.TaskName}] 진행도 : {progress} / {taskDataSO.SuccessCount}");  

        ETaskState prevState = State;
        if (progress >= taskDataSO.SuccessCount)
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
    }

    public void Register()
    { 
        State = ETaskState.Running;
    } 
} 