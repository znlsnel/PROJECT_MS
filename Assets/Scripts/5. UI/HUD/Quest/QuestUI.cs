using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class QuestUI : UIBase
{
    [SerializeField] private GameObject questSlotPrefab;
 
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

      //  Invoke(nameof(ForceRebuildLayout), 1f); 
    }
     
    private void OnQuestCompleted(Quest quest)
    {
        
    }

    private void OnQuestCanceled(Quest quest)
    {

    } 

    private void ForceRebuildLayout()
    {
   //     LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
