using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using FishNet.Object;
using UnityEngine;

public class QuestStorageBox : Interactable
{
    private static QuestStorageUI questStorageUI;


    [SerializeField] private GameObject questStorageUIPrefab;
    [SerializeField] private int questStorageIndex;
    [SerializeField] private GameObject nextQuestObject;

    private QuestStorageData questStorageData;
    private Storage storage = new Storage();
    private int successCount = 0;


    public void Awake()
    {
        if (questStorageUI == null)
        {
            questStorageUI = Instantiate(questStorageUIPrefab).GetComponent<QuestStorageUI>();
            questStorageUI.Hide();
        }

        Managers.SubscribeToInit(()=>{
            Setup(Managers.Data.questStorages.GetByIndex(questStorageIndex));
        });
    }


    public void Setup(QuestStorageData questStorageData)
    {
        if (questStorageData == null)
        {
            Debug.LogError("Not Found QuestStorageData");
            return;
        }

        this.questStorageData = questStorageData;
        foreach (var item in questStorageData.items)
        {
            int idx = storage.Count;
            QuestStorageSlot itemSlot = new QuestStorageSlot(item.amount, idx); 
            itemSlot.Setup(item.itemData);
            itemSlot.onSuccess += SuccessQuest; 
            storage.AddItemSlot(itemSlot); 
        } 

        for (int i = 0; i < storage.Count; i++)
        {
            storage.GetSlotByIdx(i).onUpdateSlot += (slotIdx) => {
                int idx = -1;

                if (storage.GetSlotByIdx(slotIdx).Data != null)
                    idx = storage.GetSlotByIdx(slotIdx).Data.Id;

                AsyncItemSlot(slotIdx, idx, storage.GetSlotByIdx(slotIdx).Stack); 
            };
        }
    }


    public override void Interact(GameObject obj)
    {
        questStorageUI.Setup(storage, questStorageData.Title);
        Managers.UI.ShowPopupUI<QuestStorageUI>(questStorageUI);
    }


    private void SuccessQuest()
    {
        successCount++;
        
        if (successCount == questStorageData.items.Count)
        {
 
            questStorageUI.Hide();
            if (nextQuestObject != null)
            {
                gameObject.SetActive(false);
                nextQuestObject.SetActive(true);
                nextQuestObject.GetComponent<QuestStorageBox>().PlaySpawnAnimation(); 
            }
            Managers.Quest.ReceiveReport(ETaskCategory.FillQuestStorage, questStorageIndex);
        }
    }


    private void PlaySpawnAnimation()
    {
        var originalScale = transform.localScale;
        transform.localScale = Vector3.one * 0.8f;


        transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack).onComplete += ()=>{
            transform.DOScale(originalScale, 0.2f).SetEase(Ease.InBack);
        };
    }
 

    [ServerRpc(RequireOwnership = false)]
    private void AsyncItemSlot(int slotIdx, int itemData, int amount)
    {
        ObserversRpcItemSlot(slotIdx, itemData, amount);  
    } 
 
    [ObserversRpc]
    private void ObserversRpcItemSlot(int slotIdx, int itemData, int amount)
    {
        ItemData data = Managers.Data.items.GetByIndex(itemData);
        storage.GetSlotByIdx(slotIdx).Setup(data, amount, true); 
    }
} 