using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotParent;
    [SerializeField] private GameObject quickSlotParent;

    private void Start()
    {
        SetItemSlots();
    }
 
    private void SetItemSlots()
    {
        Storage itemStorage = Managers.UserData.Inventory.ItemStorage;
        Storage quickStorage = Managers.UserData.Inventory.QuickSlotStorage;

        ItemSlotUI[] itemSlotUIs = itemSlotParent.GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < itemSlotUIs.Length; i++)
        {
            ItemSlot slot = itemStorage.GetSlotByIdx(i);
            if (slot == null)
                slot = itemStorage.CreateSlot();

            itemSlotUIs[i].Setup(slot); 
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


}
