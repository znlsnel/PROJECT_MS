using System;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class QuickSlotHandler : NetworkBehaviour
{
    private static readonly string _changeSlotSound = "Sound/UI/Click_06.mp3";
    

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
        SelectItem(0);
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
                SelectItem(idx);  
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
                    SelectItem(idx);
            };
    }

 
    private void SelectItem(int itemSlotIdx)
    {
        ItemSlot itemSlot = quickSlotStorage.GetSlotByIdx(itemSlotIdx);
        if (itemSlot == null || (itemSlot == selectedItemSlot && itemSlot.Data == selectedItemData)) 
            return;

        Managers.Sound.Play(_changeSlotSound);

        if (itemSlot.Data != selectedItemData) 
        {
            Broadcast_HideItem();

            if (itemSlot.Data != null)
            {
                EItemType itemType = itemSlot.Data.ItemType;
                Broadcast_ShowItem(itemSlotIdx, itemSlot.Data.PrefabPath, itemType);
                return;
            }
        } 
 
        onSelectItem?.Invoke(itemSlot, null); 
        selectedItemSlot = itemSlot;
        selectedItemData = itemSlot.Data;
    }



    [ServerRpc(RequireOwnership = false)]
    private void Broadcast_ShowItem(int slotIdx, string prefabPath, EItemType itemType)
    {
        ObserversRpcShowItem(slotIdx, prefabPath, itemType);
    }

    [ObserversRpc]
    private void ObserversRpcShowItem(int slotIdx, string prefabPath, EItemType itemType)
    {
        selectedItemObject = Managers.Pool.Get(prefabPath, transform); 
        selectedItemObject.transform.SetParent(itemType == EItemType.Weapon ? waeponRoot : itemRoot, false);
        selectedItemObject.transform.localPosition = Vector3.zero;  
        selectedItemObject.transform.localRotation = Quaternion.identity; 

        ItemSlot itemSlot = quickSlotStorage.GetSlotByIdx(slotIdx);
        if (itemSlot != null)
        {
            onSelectItem?.Invoke(itemSlot, selectedItemObject);  
            selectedItemSlot = itemSlot; 
            selectedItemData = itemSlot.Data;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void Broadcast_HideItem()
    {
       ObserversRpcHideItem();
    }

    [ObserversRpc]
    private void ObserversRpcHideItem()
    {
        if (selectedItemObject != null){
            Managers.Pool.Release(selectedItemObject);
            selectedItemObject = null;
        }
    }
}
