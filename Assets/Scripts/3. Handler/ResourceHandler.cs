using DG.Tweening;
using UnityEngine;

public class ResourceHandler : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private int dropItemCount = 1;
    public ResourceStat Hp { get; private set; }
    private Vector3 originalScale;

    public void Awake()
    {
        Hp = new ResourceStat(maxHp);
        originalScale = transform.localScale;
    }

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
            GameObject item = Managers.Pool.Get(dropItem);
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
