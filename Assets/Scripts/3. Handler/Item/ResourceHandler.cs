using System.Collections.Generic;
using DG.Tweening;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class ResourceHandler : NetworkBehaviour, IDamageable
{
    [SerializeField] private int dropItemCount = 1;

    [SerializeField] public List<int> dropItemIds = new List<int>();

    private List<GameObject> dropItems = new List<GameObject>();
    private Vector3 originalScale;

    [field: SerializeField] public HealthResource Hp {get; private set;}


    public void Awake()
    {
        Managers.SubscribeToInit(()=>{

            foreach(int itemId in dropItemIds)
                dropItems.Add(Managers.Resource.Load<GameObject>(Managers.Data.items.GetByIndex(itemId).DropPrefabPath));
            
        });
        originalScale = transform.localScale;
    }

    public override void OnStartNetwork()
    {
        Hp.OnResourceChanged += ResourceDestory;
    }

    public override void OnStopNetwork()
    {
        Hp.OnResourceChanged -= ResourceDestory;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(float damage, NetworkConnection conn = null)
    {
        DamageEffect(conn.FirstObject.gameObject);
        Hp.Subtract(damage);
    }

    [ObserversRpc]
    private void DamageEffect(GameObject attacker)
    {
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
        });
    }

    [Server]
    private void DropItem()
    {
        for(int i = 0; i < dropItemCount; i++)
        {
            GameObject item = Instantiate(dropItems[Random.Range(0, dropItems.Count)]);
            item.transform.position = transform.position;
            InstanceFinder.ServerManager.Spawn(item);

            item.GetOrAddComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    [Server]
    private void ResourceDestory(float current, float max)
    {
        if(current <= 0)
        {
            DropItem();
            InstanceFinder.ServerManager.Despawn(gameObject);
        }
    }

    public bool CanTakeDamage()
    {
        if(Hp.Current.Value <= 0)
            return false;

        return true;
    }
}
