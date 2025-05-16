using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class HUD_QuickSlotUI : MonoBehaviour
{
    [SerializeField] private Transform quickSlotRoot;


    private void Awake()
    {
        InventorySlotUI[] itemSlotUIs = quickSlotRoot.GetComponentsInChildren<InventorySlotUI>();
        
        for (int i = 0; i < itemSlotUIs.Length; i++)
        {
            ItemSlot itemSlot = Managers.UserData.Inventory.QuickSlotStorage.GetSlotByIdx(i);
            itemSlotUIs[i].Setup(itemSlot);
        }
    }
}
