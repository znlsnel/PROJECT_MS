using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : UIBase
{
    private string slotKey = "UI/Quest/QuestUI.prefab";
    private string taskKey = "UI/Quest/QuestTaskUI.prefab";

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
        Managers.Resource.LoadAllAsync<GameObject>("DefaultUI", (key, loadCount, totalCount)=>{
            if(loadCount == totalCount)
                RegisterQuestAction(); 
        });
    } 

    private void RegisterQuestAction()
    {
        Managers.Quest.onQuestRegistered += OnQuestRegistered;
        Managers.Quest.onQuestCompleted += OnQuestCompleted;
        Managers.Quest.onQuestCanceled += OnQuestCanceled;
        Managers.Quest.Register(1);   
    }
 

    private void OnQuestRegistered(Quest quest)
    {
        GameObject slot = Managers.Resource.Instantiate(slotKey);
        slot.transform.SetParent(transform, false);   
        slot.GetComponent<QuestUISlot>().Setup(quest);
        
        foreach (var task in quest.tasks)
        {
            GameObject taskUI = Managers.Resource.Instantiate(taskKey);
            taskUI.transform.SetParent(transform, false);
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
