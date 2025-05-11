using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QuestStorageBox : Interactable
{
    [SerializeField] private GameObject questStorageUIPrefab;
    private static QuestStorageUI questStorageUI;

    private Storage storage = new Storage();


    private void Awake()
    {
        if (questStorageUI == null)
        {
            questStorageUI = Instantiate(questStorageUIPrefab).GetComponent<QuestStorageUI>();
            questStorageUI.Hide();
        }

        Managers.SubscribeToInit(()=>{
            Setup(Managers.Data.questStorages.GetByIndex(1001));
        });
    }


    public void Setup(QuestStorageData questStorageData)
    {
        foreach (var item in questStorageData.items)
        {
            QuestStorageSlot itemSlot = new QuestStorageSlot(item.amount);
            itemSlot.Setup(item.itemData);
            storage.AddItemSlot(itemSlot);
        }
    }


    public override void Interact(GameObject obj)
    {
        questStorageUI.Setup(storage);

        Managers.UI.ShowPopupUI<QuestStorageUI>(questStorageUI);
    }
}