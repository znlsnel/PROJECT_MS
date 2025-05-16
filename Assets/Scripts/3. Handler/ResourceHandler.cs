using System.Collections.Generic;
using DG.Tweening;
using FishNet.Object;
using UnityEngine;

public class ResourceHandler : NetworkBehaviour, IDamageable
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private int dropItemCount = 1;

    [SerializeField] public List<int> dropItemIds = new List<int>();

    private List<GameObject> dropItems;
    private Vector3 originalScale;

    public ResourceStat Hp { get; private set; }


    public void Awake()
    {
        Managers.SubscribeToInit(()=>{

            foreach(int itemId in dropItemIds)
                dropItems.Add(Managers.Resource.Load<GameObject>(Managers.Data.items.GetByIndex(itemId).DropPrefabPath));
            
        });
        Hp = new ResourceStat(maxHp);
        originalScale = transform.localScale;
    }

    [ServerRpc]
    public void TakeDamage(float damage, GameObject attacker)
    {
     //   Hp.Subtract(damage);
        Hp.Subtract(30);
        
        transform.DOKill();

        Vector3 attDir = (transform.position - attacker.transform.position).normalized;
        Vector3 originPos = transform.position;
        transform.localScale = originalScale;

        
        transform.DOMove(originPos + attDir * 0.1f, 0.1f).SetEase(Ease.OutCubic).OnComplete(() => 
        {
            transform.DOMove(originPos, 0.1f).SetEase(Ease.InCubic);
        });

        transform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.OutCubic).OnComplete(() => 
        {
            transform.DOScale(originalScale, 0.1f).SetEase(Ease.InCubic);
            if(Hp.Current <= 0)
            {
                ResourceDestory();
                return;
            }
        });
        
    }

    private void DropItem()
    {
        for(int i = 0; i < dropItemCount; i++)
        {
            GameObject item = Managers.Pool.Get(dropItems[Random.Range(0, dropItems.Count)]);
            item.transform.position = transform.position;

            item.GetOrAddComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    private void ResourceDestory()
    {
        transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutQuint).OnComplete(() => 
        {
            DropItem();
            Destroy(gameObject);
        });
    }
}
