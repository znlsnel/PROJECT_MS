using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipHandler : MonoBehaviour
{
    [SerializeField] private Transform equipParent;
    [SerializeField] private Transform bone;

    private Dictionary<EEquipType, GameObject> equipObjects = new Dictionary<EEquipType, GameObject>(){
        {EEquipType.Head, null},
        {EEquipType.Shirt, null},
        {EEquipType.Pants, null},
        {EEquipType.Shoes, null}
    };
 
    private void Awake()
    {
        EquipStorage equipStorage = Managers.UserData.Inventory.EquipStorage;

        foreach (EEquipType equipType in Enum.GetValues(typeof(EEquipType)))
        {
            ItemSlot itemSlot = equipStorage.GetSlotByType(equipType);
            itemSlot.onChangeStack += OnChangeEquip;
        }
    }


    private void OnChangeEquip(ItemSlot itemSlot)
    {

        GameObject equipObject = equipObjects[itemSlot.slotEquipType];
        if (equipObject != null){
            Managers.Resource.Destroy(equipObject);  
        }

        equipObjects[itemSlot.slotEquipType] = null;

        if (itemSlot.Data == null)
            return;

        equipObject = Managers.Pool.Get(itemSlot.Data.PrefabPath, equipParent);
        equipObjects[itemSlot.slotEquipType] = equipObject;

        equipObject.transform.localPosition = Vector3.zero;
        equipObject.transform.localRotation = Quaternion.identity;

        SkinnedMeshUpdater.UpdateMeshRenderer(bone, equipObject.GetComponentInChildren<SkinnedMeshRenderer>()); 
    }
}
