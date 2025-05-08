using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트의 ID값, 퀘스트의 상태값, Task의 ID값과 진행도를 저장 => 리스트의 형태로
public class Quest
{
  public QuestData QuestData {get; private set;} 
  public List<QuestTask> tasks {get; private set;}
 
  public EQuestState State { get; private set; } = EQuestState.Inactive;

  public bool IsRegistered => State != EQuestState.Inactive;
  public bool IsComplatable => State == EQuestState.WaitingForCompletion;
  public bool IsComplete => State == EQuestState.Complete;
  public bool IsCancel => State == EQuestState.Cancel;
  
  private int completedTaskCount = 0;
  
 // Event
  public event Action<Quest> onWaitingForCompletion;
  public event Action<Quest> onCompleted;  


  public void Register(QuestData questData)
  {
    this.QuestData = questData;
    State = EQuestState.Running;

    tasks = new List<QuestTask>();

    foreach (var taskData in questData.Tasks)
    {
      tasks.Add(new QuestTask());
      tasks.Back().Initialize(taskData); 
      tasks.Back().onSuccessChanged += SuccessChangedTask;
    }
  } 


  public void RecieveReport(ETaskCategory category, int targetId, int successCount)
  {
    foreach (var task in tasks)
    {
      if (task.IsMatch(category, targetId))
        task.ReceiveReport(successCount);
      
    }
  } 

  private void SuccessChangedTask(QuestTask task, bool isSuccess)
  {
    completedTaskCount += isSuccess ? 1 : -1;

    Debug.Log($"[{QuestData.Title}] 퀘스트 진행도 : {completedTaskCount}/{QuestData.Tasks.Count}");
    if (completedTaskCount >= QuestData.Tasks.Count)
    {
      State = EQuestState.WaitingForCompletion;
      
      if (QuestData.UseAutoComplete)
          Complete(); 
        else 
          onWaitingForCompletion?.Invoke(this);  
    }
  } 
 
  private void Complete() 
  {
    State = EQuestState.Complete;
    onCompleted?.Invoke(this); 
    Debug.Log($"[{QuestData.Title}] 퀘스트 완료"); 
  }
 
  public void Cancel()
  {
    // TODO 퀘스트 삭제 처리 

  }

}