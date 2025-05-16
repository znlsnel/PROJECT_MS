using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTask
{
    public TaskData taskData {get; private set;}
    public ETaskState State { get; private set; } = ETaskState.Inactive;
    public int progress {get; private set;}
 
    public event Action<QuestTask, bool> onSuccessChanged;
    public event Action<QuestTask, int, int> onProgressChanged;
    public event Action<QuestTask> onMoveToNextTask;
   

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
        Debug.Log($"[{taskData.TaskTitle}] 진행도 : {progress} / {taskData.SuccessCount}");  

        ETaskState prevState = State;
        if (progress >= taskData.SuccessCount)
        {
            State = ETaskState.Complete;
            if (prevState != ETaskState.Complete)
            {
                // 연결된 작업이 있다면 새로 업데이트
                TaskData linkedTaskData = Managers.Data.questTasks.GetByIndex(taskData.LinkedTaskId);
                if (linkedTaskData != null)
                    MoveToNextTask(linkedTaskData);
                else
                    onSuccessChanged?.Invoke(this, true);   
            }
        }
        else
        {
            State = ETaskState.Running;
            if (prevState == ETaskState.Complete)
                onSuccessChanged?.Invoke(this, false);   
        }

        onProgressChanged?.Invoke(this, progress, taskData.SuccessCount); 
    }


    private void MoveToNextTask(TaskData linkedTaskData)
    {
        taskData = linkedTaskData;
        progress = 0;
        State = ETaskState.Inactive;
        onMoveToNextTask?.Invoke(this);
    }

} 