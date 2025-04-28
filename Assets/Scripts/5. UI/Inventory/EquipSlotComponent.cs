using UnityEngine;

[RequireComponent(typeof(ItemSlotUI))]
public class EquipSlotComponent : MonoBehaviour
{
   [SerializeField] private EEquipType equipType; 
   public EEquipType EquipType => equipType;

   public void Awake()
   {
      GetComponent<ItemSlotUI>().onSetup += Setup;
   }

   private void Setup(ItemSlot itemSlot)
   {
      itemSlot.slotCondition = (itemData) => itemData.EquipType == equipType; 
      itemSlot.slotEquipType = equipType; 
   }
} 
 