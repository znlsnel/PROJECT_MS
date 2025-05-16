using UnityEngine;
using DG.Tweening;

public class ItemObject : Interactable
{
    [SerializeField] public int itemId;

    private ItemData itemData;
    void Awake()
    {
        itemData = Managers.Data.items.GetByIndex(itemId);
    }
    public override void Interact(GameObject obj)
    {
        Managers.UserData.Inventory.AddItem(itemData, 1);

        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
