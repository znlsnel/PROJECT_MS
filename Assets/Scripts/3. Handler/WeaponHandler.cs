using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [field: SerializeField] public AnimationClip attackAnimation { get; private set; }
    [field: SerializeField] public float attackAnimationSpeed { get; private set; } = 1f;

    private HashSet<IDamageable> damageables = new HashSet<IDamageable>();
    [SerializeField] private float damage = 10f;
    private bool isAttacking;

    public GameObject Owner { get; private set; }

    public void Awake()
    {
        Owner = GetComponentInParent<AlivePlayer>().gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!isAttacking)
            return;
        
        if(other.gameObject == Owner)
            return;

        if(other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            if(damageables.Add(damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log("Damaged");
            }
        }
    }

    public void SetIsAttacking(bool isAttacking)
    {
        if(this.isAttacking != isAttacking)
        {
            this.isAttacking = isAttacking;
            damageables.Clear();
        }
    }
}
