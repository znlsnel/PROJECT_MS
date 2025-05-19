using System.Collections.Generic;
using UnityEngine;

public class WeaponController : ItemController
{
    [field: SerializeField] public AnimationClip holdAnimation { get; private set; }
    [field: SerializeField] public AnimationClip attackAnimation { get; private set; }
    [field: SerializeField] public float attackAnimationSpeed { get; private set; } = 1f;

    private HashSet<IDamageable> damageables = new HashSet<IDamageable>();
    [SerializeField] private float damage = 10f;
    private bool isAttacking;

    public AlivePlayer Owner { get; private set; }

    public int HoldAnimationIndex { get; private set; }
    public int AttackAnimationIndex { get; private set; }

    public void OnValidate()
    {
        if(holdAnimation != null)
        {
            HoldAnimationIndex = AnimationDataManager.AddAnimationClip(holdAnimation);
        }
        if(attackAnimation != null)
        {
            AttackAnimationIndex = AnimationDataManager.AddAnimationClip(attackAnimation);
        }
    }

    public void Start()
    {
        Owner = GetComponentInParent<AlivePlayer>();
        Owner.ChangeWeapon(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!isAttacking)
            return;
        
        if(other.gameObject == Owner.gameObject)
            return;

        Debug.Log("TriggerEnter");

        if(other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            if(damageables.Add(damageable))
            {
                damageable.TakeDamage(damage, Owner.gameObject);
                itemSlot.UseDurability(1f);
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

    public override void OnAction()
    {
        // 공격 시작
    }

}
