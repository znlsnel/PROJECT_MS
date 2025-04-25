using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public int ItemId { get; private set; }


    public void Interact()
    {
        Managers.Quest.ReceiveReport(ETaskCategory.Pickup, ItemId);
        Destroy(gameObject);
    }

    public void Setup(int itemId)
    {
        ItemId = itemId;
    } 
}
