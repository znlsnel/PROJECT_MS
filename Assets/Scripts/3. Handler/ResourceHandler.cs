using DG.Tweening;
using UnityEngine;

public class ResourceHandler : MonoBehaviour, IDamageable
{
    public ResourceStat Hp { get; private set; } = new ResourceStat(100f);

    public void TakeDamage(float damage)
    {
        Hp.Subtract(damage);
        transform.DOScale(0.8f, 0.1f).OnComplete(() => transform.DOScale(1f, 0.1f));
    }
}
