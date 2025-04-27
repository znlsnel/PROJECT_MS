using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public int ItemId { get; private set; }
    private ItemData itemData;
    void Awake()
    {
        Managers.SubscribeToInit(() =>
        {
            itemData = new ItemData(Managers.Data.items.GetByIndex(ItemId)); 
        });
    }

    public GameObject Interact(GameObject player)
    {
        Managers.Quest.ReceiveReport(ETaskCategory.Pickup, ItemId);
        Managers.UserData.Inventory.AddItem(itemData);
        Destroy(gameObject);
        return null;
    } 

    public void Setup(int itemId)
    {
        ItemId = itemId;
    } 
}
