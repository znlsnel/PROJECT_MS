using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestManager : IManager
{
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();


    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;


    public event Action<Quest> onQuestRegistered;
    public event Action<Quest> onQuestCompleted;
    public event Action<Quest> onQuestCanceled;

 
    public void Init() 
    {

    }

    public void Clear()
    {
        activeQuests.Clear();
        completedQuests.Clear(); 
    }

    // 퀘스트의 ID값, 퀘스트의 상태값, Task의 ID값과 진행도를 저장 => 리스트의 형태로
    public Quest Register(int questId)
    {
        QuestData questData = Managers.Data.quests.GetByIndex(questId);
        return Register(questData);
    }

    public Quest Register(QuestData questData)
    {
        Quest quest = new Quest();
        quest.Register(questData); 
        quest.onCompleted += OnQuestCompleted;

        activeQuests.Add(quest);

        onQuestRegistered?.Invoke(quest);

        return quest;
    }

    public void ReceiveReport(ETaskCategory category, int targetId, int successCount = 1)
    {
        foreach (var quest in activeQuests.ToArray())
            quest.RecieveReport(category, targetId, successCount); 
    }
    
    private void OnQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    public void QuestCancel(QuestData questData) => QuestCancel(activeQuests.Find(quest => quest.QuestData == questData));
    public void QuestCancel(Quest quest)
    {
        activeQuests.Remove(quest);
        onQuestCanceled?.Invoke(quest); 
    }
    
    
} 
