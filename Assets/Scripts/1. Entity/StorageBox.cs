using FishNet.Object;
using UnityEngine;

public class StorageBox : Interactable
{
    [SerializeField] private GameObject storageBoxPrefab;

    private static StorageBoxUI storageBoxUI;
    private Storage storage = new Storage(30); 
    void Awake() 
    {
        Managers.SubscribeToInit(()=>{
            if (storageBoxUI == null)
            {
                storageBoxUI = Instantiate(storageBoxPrefab).GetComponent<StorageBoxUI>();
                storageBoxUI.Hide();  
            } 

            for (int i = 0; i < storage.Count; i++)
            {
                storage.GetSlotByIdx(i).onUpdateSlot += (slotIdx) => {

                    int idx = -1;

                    if (storage.GetSlotByIdx(slotIdx).Data != null)
                        idx = storage.GetSlotByIdx(slotIdx).Data.Id;

                    AsyncItemSlot(slotIdx, idx, storage.GetSlotByIdx(slotIdx).Stack); 
                }; 
            }
        });
    }

    public override void Interact(GameObject obj)
    {
        if (!storageBoxUI.IsOpen)
        {
            storageBoxUI.Setup(storage); 
            Managers.UI.ShowPopupUI<StorageUI>(storageBoxUI);
        } 
    }

    [ServerRpc(RequireOwnership = false)]
    private void AsyncItemSlot(int slotIdx, int itemData, int amount)
    {
        ObserversRpcItemSlot(slotIdx, itemData, amount); 
    }
 
    [ObserversRpc]
    private void ObserversRpcItemSlot(int slotIdx, int itemData, int amount)
    {
        ItemData data = Managers.Data.items.GetByIndex(itemData);
        storage.GetSlotByIdx(slotIdx).Setup(data, amount, true);  
    }
}

