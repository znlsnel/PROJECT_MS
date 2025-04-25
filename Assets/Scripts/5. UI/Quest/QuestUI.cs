using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : UIBase
{
    [SerializeField] private GameObject questSlotPrefab;
    [SerializeField] private GameObject questTaskPrefab;
 
    protected override void Awake()
    {
        base.Awake();
        Managers.SubscribeToInit(Init); 
    }

    private void OnDestroy()
    {
        if (Managers.IsNull) return;

        Managers.Quest.onQuestRegistered -= OnQuestRegistered;
        Managers.Quest.onQuestCompleted -= OnQuestCompleted;
        Managers.Quest.onQuestCanceled -= OnQuestCanceled;
    }

    private void Init()
    {
        Managers.Quest.onQuestRegistered += OnQuestRegistered;
        Managers.Quest.onQuestCompleted += OnQuestCompleted;
        Managers.Quest.onQuestCanceled += OnQuestCanceled;
    } 
 
 
    private void OnQuestRegistered(Quest quest)
    {
        GameObject slot = Instantiate(questSlotPrefab, transform);
        slot.GetComponent<QuestUISlot>().Setup(quest);
        
        foreach (var task in quest.tasks)
        {
            GameObject taskUI = Instantiate(questTaskPrefab, transform);
            taskUI.GetComponent<QuestTaskUI>().Setup(task);
        } 
    }
    
    private void OnQuestCompleted(Quest quest)
    {
        
    }

    private void OnQuestCanceled(Quest quest)
    {

    }
}
