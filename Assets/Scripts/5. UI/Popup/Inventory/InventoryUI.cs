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


    private List<InventorySlotUI> itemSlots = new List<InventorySlotUI>();
    
    private List<ItemData> testItems = new List<ItemData>();

    private void Start()
    {
        SetItemSlots();
        RegisterInput();
    }
 
    private void RegisterInput()
    {
        testItems.Add(Managers.Data.items.GetByIndex(2001)); 
        testItems.Add(Managers.Data.items.GetByIndex(1001)); 


  
        


        
        Managers.Input.Test.started += TestInput;
    }

    private void TestInput(InputAction.CallbackContext context)
    {
        Managers.UserData.Inventory.AddItem(testItems[Random.Range(0, testItems.Count)]); 
    }

    private void SetItemSlots()
    {
        Storage itemStorage = Managers.UserData.Inventory.ItemStorage;
        Storage quickStorage = Managers.UserData.Inventory.QuickSlotStorage;
        EquipStorage equipStorage = Managers.UserData.Inventory.EquipStorage;


        // 아이템 슬롯 초기화
        InventorySlotUI[] slots = itemSlotParent.GetComponentsInChildren<InventorySlotUI>();
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot slot = itemStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = itemStorage.CreateSlot();

            slots[i].Setup(slot); 
            itemSlots.Add(slots[i]); 
        }

        // 퀵슬로 초기화
        InventorySlotUI[] quickSlotUIs = quickSlotParent.GetComponentsInChildren<InventorySlotUI>();
        for (int i = 0; i < quickSlotUIs.Length; i++)
        {
            ItemSlot slot = quickStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = quickStorage.CreateSlot();
 
            quickSlotUIs[i].Setup(slot);  
        } 

        // 장착 슬롯 초기화
        InventorySlotUI[] equipSlotUIs = equipSlotParent.GetComponentsInChildren<InventorySlotUI>();
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
