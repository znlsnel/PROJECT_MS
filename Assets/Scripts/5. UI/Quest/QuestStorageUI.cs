using UnityEngine;

public class QuestStorageUI : StorageUI
{

    private QuestStorageSlotUI[] slots;

    protected override void Awake()
    {
        base.Awake();
        slots = storageRoot.GetComponentsInChildren<QuestStorageSlotUI>();
    }

    public void Setup(Storage storage)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot itemSlot = storage.GetSlotByIdx(i);
            slots[i].gameObject.SetActive(itemSlot != null);
            slots[i].Setup(itemSlot);
        }
    }
}
