using System;
using FishNet;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotHandler : NetworkBehaviour
{
    [SerializeField] private Transform itemRoot;
    [SerializeField] private Transform waeponRoot;

    private Storage quickSlotStorage;

    private ItemSlot selectedItemSlot;
    private ItemData selectedItemData;
    private GameObject selectedItemObject;

    public event Action<ItemSlot, GameObject> onSelectItem;


    public void Setup(Inventory inventory)
    {
        quickSlotStorage = inventory.QuickSlotStorage;

        InitInput();
        InitQuickSlot();
        RequestSelectItem(0);
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
                RequestSelectItem(idx);  
            };
        }
    }

    private void InitQuickSlot()
    {
        // 선택한 퀵슬롯이 변경되면 새로 업데이트
        for (int i = 0; i < quickSlotStorage.Count; i++)
            quickSlotStorage.GetSlotByIdx(i).onChangeStack += (ItemSlot itemSlot) => 
            {
                int idx = i;
                if (selectedItemSlot == itemSlot && itemSlot.Data != selectedItemData)
                    RequestSelectItem(idx);
            };
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestSelectItem(int itemSlotIdx )
    {
        ObserversRpcSelectItem(itemSlotIdx);
    }

    [ObserversRpc]
    private void ObserversRpcSelectItem(int itemSlotIdx)
    {
        ItemSlot itemSlot = quickSlotStorage.GetSlotByIdx(itemSlotIdx);
        if (itemSlot == null || (itemSlot == selectedItemSlot && itemSlot.Data == selectedItemData)) 
            return;


        if (itemSlot.Data != selectedItemData) 
        {
            HideItem();

            if (itemSlot.Data != null)
            {
                EItemType itemType = itemSlot.Data.ItemType;
                ShowItem(itemSlot, itemType);
            }
        } 


        onSelectItem?.Invoke(itemSlot, selectedItemObject); 
        selectedItemSlot = itemSlot;
        selectedItemData = itemSlot.Data;
    }



    public void ShowItem(ItemSlot itemSlot, EItemType itemType)
    {
        string PrefabPath = itemSlot.Data.PrefabPath;
        selectedItemObject = Managers.Pool.Get(PrefabPath, transform); 

        selectedItemObject.transform.SetParent(itemType == EItemType.Weapon ? waeponRoot : itemRoot, false);

        selectedItemObject.transform.localPosition = Vector3.zero;  
        selectedItemObject.transform.localRotation = Quaternion.identity; 

        
    }



    public void HideItem()
    {
        if (selectedItemObject != null){
            Managers.Pool.Release(selectedItemObject);
            selectedItemObject = null;
        }
    }
}
