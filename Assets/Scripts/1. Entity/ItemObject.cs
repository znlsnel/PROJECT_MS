using UnityEngine;
using DG.Tweening;
using UGS;
using FishNet.Object;

public class ItemObject : Interactable
{
    [SerializeField] public int itemId;

    private ItemData itemData;

    public void Awake()
    {
        Managers.SubscribeToInit(()=>{
            itemData = Managers.Data.items.GetByIndex(itemId);
        });
    }

    [ServerRpc(RequireOwnership = false)]
    public override void Interact(GameObject obj)
    {
        InteractRpc(obj);
    }

    [ObserversRpc]
    private void InteractRpc(GameObject obj)
    {
        AlivePlayer player = obj.GetComponent<AlivePlayer>();
        player.Inventory.AddItem(itemData, 1);

        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}

// 1. 구성 작성 -> 버튼 등등
// 2. 로직 작성 -> 버튼을 클릭했을 때의 로직