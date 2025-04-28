using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipHandler : MonoBehaviour
{
    [SerializeField] private Transform hairParent;
    [SerializeField] private Transform shirtParent;
    [SerializeField] private Transform pantsParent;
    [SerializeField] private Transform shoesParent; 

    private Dictionary<EEquipType, Transform> equipParents;
    private Dictionary<EEquipType, GameObject> equipObjects;

    private void Awake()
    {
        equipParents = new Dictionary<EEquipType, Transform>()
        {
            {EEquipType.Hair, hairParent},
            {EEquipType.Shirt, shirtParent},
            {EEquipType.Pants, pantsParent},
            {EEquipType.Shoes, shoesParent}
        };

        EquipStorage equipStorage = Managers.UserData.Inventory.EquipStorage;

        foreach (EEquipType equipType in Enum.GetValues(typeof(EEquipType)))
        {
            ItemSlot itemSlot = equipStorage.GetSlotByType(equipType);
            itemSlot.onChangeStack += OnChangeEquip;
        }
    }

    private void OnChangeEquip(ItemSlot itemSlot)
    {
        if (itemSlot.Data == null)
            return;

        Transform parent = equipParents[itemSlot.slotEquipType];

        GameObject equipObject = equipObjects[itemSlot.slotEquipType];
        if (equipObject != null)
            Managers.Resource.Destroy(equipObject); 
 
        Managers.Resource.LoadAsync<GameObject>(itemSlot.Data.PrefabPath, (gameObject) =>
        {
            equipObject = Instantiate(gameObject, parent, false);  
            equipObjects[itemSlot.slotEquipType] = equipObject; 
        });
    }

}
