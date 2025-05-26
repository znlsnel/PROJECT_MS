using TMPro;
using UnityEngine;

public class QuestStorageUI : StorageUI
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private CloseButton closeButton;
    private QuestStorageSlotUI[] slots;

    protected override void Awake()
    {
        base.Awake();
        closeButton.OnClick += Hide;
        slots = storageRoot.GetComponentsInChildren<QuestStorageSlotUI>();
    }

    public void Setup(Storage storage, string title)
    {
        _title.text = title;
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot itemSlot = storage.GetSlotByIdx(i);
            slots[i].gameObject.SetActive(itemSlot != null);
            slots[i].Setup(itemSlot);
        }
    } 
}
