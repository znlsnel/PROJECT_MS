using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryUI : PopupUI
{
    [SerializeField] private GameObject itemSlotParent;
    [SerializeField] private GameObject quickSlotParent;
    [SerializeField] private GameObject equipSlotParent;
    [SerializeField] private GameObject mainPanel;


    private List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();
    
    private List<ItemData> testItems = new List<ItemData>();

    private Inventory inventory;


    public void Setup(AlivePlayer player)
    {
        inventory = player.Inventory;
        SetItemSlots();
        RegisterInput();
    }

    public void Close()
    {
        HideWithDoTween(mainPanel.transform);   
    }

    private void RegisterInput()
    {
        #if UNITY_EDITOR
        testItems.Add(Managers.Data.items.GetByIndex(3001));
        testItems.Add(Managers.Data.items.GetByIndex(3007));
        testItems.Add(Managers.Data.items.GetByIndex(3003));
        testItems.Add(Managers.Data.items.GetByIndex(1001));
        testItems.Add(Managers.Data.items.GetByIndex(3005));
        testItems.Add(Managers.Data.items.GetByIndex(3006)); 
        Managers.Input.Test.started += TestInput;
        #endif 
    }

    private void TestInput(InputAction.CallbackContext context)
    {
        ItemData itemData = testItems[Random.Range(0, testItems.Count)];
        inventory.AddItem(itemData, 1, (int)itemData.MaxDurability);  
        MyDebug.Log($"아이템 추가"); 

    }

    private void SetItemSlots()
    {
        Storage itemStorage = inventory.ItemStorage;
        Storage quickStorage = inventory.QuickSlotStorage;
        EquipStorage equipStorage = inventory.EquipStorage;
 

        // 아이템 슬롯 초기화
        ItemSlotUI[] slots = itemSlotParent.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot slot = itemStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = itemStorage.CreateSlot();

            slots[i].Setup(slot); 
            itemSlots.Add(slots[i]); 
        }

        // 퀵슬로 초기화
        ItemSlotUI[] quickSlotUIs = quickSlotParent.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < quickSlotUIs.Length; i++)
        {
            ItemSlot slot = quickStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = quickStorage.CreateSlot();
 
            quickSlotUIs[i].Setup(slot);  
        } 

        // 장착 슬롯 초기화
        ItemSlotUI[] equipSlotUIs = equipSlotParent.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < equipSlotUIs.Length; i++)
        {
            EEquipType equipType = equipSlotUIs[i].GetComponent<EquipSlotComponent>().EquipType;
            ItemSlot slot = equipStorage.GetSlotByType(equipType);   
            equipSlotUIs[i].Setup(slot);   
        }
    }
 
    public void FilterInventoryByType(EItemType itemType)
    {
        if (itemType == EItemType.None)
        {
            itemSlots.ForEach(x => x.gameObject.SetActive(true));
            return;
        }
 
        itemSlots.FindAll(x => x.ItemSlot.Data == null).ForEach(x => x.gameObject.SetActive(false));
        itemSlots.FindAll(x => x.ItemSlot.Data != null && x.ItemSlot.Data.ItemType == itemType).ForEach(x => x.gameObject.SetActive(true));
        itemSlots.FindAll(x => x.ItemSlot.Data != null && x.ItemSlot.Data.ItemType != itemType).ForEach(x => x.gameObject.SetActive(false));
    }



}
