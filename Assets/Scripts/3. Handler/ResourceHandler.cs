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

    public void TakeDamage(float damage)
    {
        Hp.Subtract(damage);
        if(Hp.Current <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
        else
        {
            transform.DOScale(originalScale * 0.8f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f));
        }
    }

    private void DropItem()
    {
        for(int i = 0; i < dropItemCount; i++)
        {
            GameObject item = Managers.Pool.Get(dropItem);
            item.transform.position = transform.position;
        }
    }
}
