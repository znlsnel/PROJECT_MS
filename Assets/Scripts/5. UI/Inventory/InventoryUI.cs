using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotParent;
    [SerializeField] private GameObject quickSlotParent;

    private List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();

    private void Start()
    {
        SetItemSlots();
    }
 
    private void SetItemSlots()
    {
        Storage itemStorage = Managers.UserData.Inventory.ItemStorage;
        Storage quickStorage = Managers.UserData.Inventory.QuickSlotStorage;

        ItemSlotUI[] slots = itemSlotParent.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot slot = itemStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = itemStorage.CreateSlot();

            slots[i].Setup(slot); 
            itemSlots.Add(slots[i]); 
        }

        ItemSlotUI[] quickSlotUIs = quickSlotParent.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < quickSlotUIs.Length; i++)
        {
            ItemSlot slot = quickStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = quickStorage.CreateSlot();
 
            quickSlotUIs[i].Setup(slot);  
        } 
    }
 
    public void FilterInventoryByType(EItemType itemType)
    {
        if (itemType == EItemType.None)
        {
            itemSlots.ForEach(x => x.gameObject.SetActive(true));
            return;
        }

        itemSlots.FindAll(x => x.ItemSlot.Data != null && x.ItemSlot.Data.ItemType == itemType).ForEach(x => x.gameObject.SetActive(true));
        itemSlots.FindAll(x => x.ItemSlot.Data != null && x.ItemSlot.Data.ItemType != itemType).ForEach(x => x.gameObject.SetActive(false));
    }



}
