using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotHandler : MonoBehaviour
{
    [SerializeField] private Transform itemRoot;
    [SerializeField] private Transform waeponRoot;

    private Storage quickSlotStorage;

    private ItemSlot selectedItemSlot;
    private ItemData selectedItemData;
    private GameObject selectedItemObject;

    public event Action<ItemData> onSelectItem;

    private void Awake()
    {
        quickSlotStorage = Managers.UserData.Inventory.QuickSlotStorage;

        Managers.SubscribeToInit(InitInput);
        Managers.SubscribeToInit(InitQuickSlot);
    }

    private void InitInput()
    {
        // 퀵 슬롯 키 할당
        for (int i = 0; i < Enum.GetValues(typeof(EQuickSlot)).Length; i++)
        {
            EQuickSlot quickSlot = (EQuickSlot)i;
            int idx = i;

            Managers.Input.GetInput(quickSlot).started += (InputAction.CallbackContext context) =>
            {
                SelectItem(quickSlotStorage.GetSlotByIdx(idx));  
            };
        }
    }

    private void InitQuickSlot()
    {
        // 선택한 퀵슬롯이 변경되면 새로 업데이트
        for (int i = 0; i < quickSlotStorage.Size; i++)
            quickSlotStorage.GetSlotByIdx(i).onChangeStack += (ItemSlot itemSlot) => 
            {
                if (selectedItemSlot == itemSlot && itemSlot.Data != selectedItemData)
                    SelectItem(itemSlot);
            };
    }

    private void SelectItem(ItemSlot itemSlot)
    {
        if (itemSlot == null || (itemSlot == selectedItemSlot && itemSlot.Data == selectedItemData)) 
            return;

        if (itemSlot.Data == null)
            return;


        if (itemSlot.Data != selectedItemData) 
        {
            if (selectedItemObject != null)
                Managers.Resource.Destroy(selectedItemObject);  

            EItemType itemType = itemSlot.Data.ItemType;
            selectedItemObject = Managers.Pool.Get(itemSlot.Data.PrefabPath, transform); 
            selectedItemObject.transform.SetParent(itemType == EItemType.Weapon ? waeponRoot : itemRoot, false);

            selectedItemObject.transform.localPosition = Vector3.zero;  
            selectedItemObject.transform.localRotation = Quaternion.identity; 
        } 

        selectedItemSlot = itemSlot;
        selectedItemData = itemSlot.Data;
    }


}
