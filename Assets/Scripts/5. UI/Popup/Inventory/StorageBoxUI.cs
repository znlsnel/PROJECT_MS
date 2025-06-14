using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class StorageBoxUI : StorageUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private GameObject mainPanel;

    protected override void Awake() 
    {
        base.Awake();
        closeButton.OnClick += () => HideWithDoTween(mainPanel.transform); 

    }
 
    public void Setup(Storage storage)
    {

        ItemSlotUI[] storageSlots = storageRoot.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < storageSlots.Length; i++)
        {
            storageSlots[i].UnSetup();

            ItemSlot itemSlot = storage.GetSlotByIdx(i);
            if (itemSlot == null)
                itemSlot = storage.CreateSlot();

            storageSlots[i].Setup(itemSlot); 
        }
    }

}
