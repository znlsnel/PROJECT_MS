using UnityEngine;
using DG.Tweening;
using UGS;
using FishNet.Object;
using FishNet.Object.Synchronizing;


public class ItemObject : Interactable
{
    [SerializeField] public int itemId;
    private ItemData itemData;
    private bool isPlayingDoTween = false;
    public override bool isActive => base.isActive && !isPlayingDoTween; 

    public int Durability {get; private set;} = 0;   



    private void Awake() 
    {
        Managers.SubscribeToInit(()=>
        {
            itemData = Managers.Data.items.GetByIndex(itemId);
            Durability = (int)itemData.MaxDurability;      
        }); 
    }


    public void SetDurability(int durability)
    {
        Durability = durability; 
 
    }





    public void OnEnable()
    {
        isPlayingDoTween = false; 
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
        player.Inventory.AddItem(itemData, 1, Durability);  
        isPlayingDoTween = true;

        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine);

        transform.DOMoveY(transform.position.y + 1, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            isPlayingDoTween = false;
            Destroy(gameObject); 
        });
    }


}

// 1. 구성 작성 -> 버튼 등등
// 2. 로직 작성 -> 버튼을 클릭했을 때의 로직