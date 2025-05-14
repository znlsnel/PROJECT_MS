using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
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


    private void Awake()
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
            QuestStorageSlot itemSlot = new QuestStorageSlot(item.amount);
            itemSlot.Setup(item.itemData);
            itemSlot.onSuccess += SuccessQuest;
            storage.AddItemSlot(itemSlot);
        }
    }


    public override void Interact(GameObject obj)
    {
        questStorageUI.Setup(storage);
        Managers.UI.ShowPopupUI<QuestStorageUI>(questStorageUI);
    }


    private void SuccessQuest()
    {
        successCount++;
        
        if (successCount == questStorageData.items.Count)
        {

            Managers.UI.CloseAllPopupUI();
            if (nextQuestObject != null)
            {
                gameObject.SetActive(false);
                nextQuestObject.SetActive(true);
                nextQuestObject.GetComponent<QuestStorageBox>().PlaySpawnAnimation(); 
            }
            Managers.Quest.ReceiveReport(ETaskCategory.FillQuestStorage, 1001);
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
}