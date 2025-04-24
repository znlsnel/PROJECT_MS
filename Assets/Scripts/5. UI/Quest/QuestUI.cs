using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : UIBase
{
    private string slotKey = "UI/QuestUI.prefab";

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
        Managers.Resource.LoadAsync<GameObject>(slotKey);
        Managers.Quest.onQuestRegistered += OnQuestRegistered;
        Managers.Quest.onQuestCompleted += OnQuestCompleted;
        Managers.Quest.onQuestCanceled += OnQuestCanceled;

        Managers.SubscribeToInit(()=>Managers.Quest.Register(1)); 
    }

    private void OnQuestRegistered(Quest quest)
    {
        GameObject slot = Managers.Resource.Instantiate(slotKey);
        slot.transform.SetParent(transform, false);  
    }
    
    private void OnQuestCompleted(Quest quest)
    {
        
    }

    private void OnQuestCanceled(Quest quest)
    {

    }
}
