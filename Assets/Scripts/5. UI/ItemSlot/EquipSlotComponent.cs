using UnityEngine;

[RequireComponent(typeof(InventorySlotUI))]
public class EquipSlotComponent : MonoBehaviour
{
   [SerializeField] private EEquipType equipType; 
   public EEquipType EquipType => equipType;

   public void Awake()
   {
      GetComponent<InventorySlotUI>().onSetup += Setup;
   }

   private void Setup(ItemSlot itemSlot)
   {
      itemSlot.slotCondition = (itemData) => itemData == null ? true : itemData.EquipType == equipType; 
      itemSlot.slotEquipType = equipType;
   }
} 
 