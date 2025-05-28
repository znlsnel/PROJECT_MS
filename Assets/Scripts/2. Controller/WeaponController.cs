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

    private readonly static string fxHumanPath = "FX/PunchFX_Human.prefab";
    private readonly static string fxPath = "FX/PunchFX.prefab";  


    private static GameObject _fxHuman;
    private static GameObject _fx;
 
    private void Start()
    {
        if (_fxHuman == null)
        {
            _fxHuman = Resources.Load<GameObject>(fxHumanPath);
        }
        if (_fx == null)
        {
            _fx = Resources.Load<GameObject>(fxPath);
        }
    }

    public override void Setup(AlivePlayer owner, ItemSlot itemSlot)
    {
        base.Setup(owner, itemSlot);
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
            GameObject fx = null;
            if (other.gameObject.GetComponent<AlivePlayer>() != null)
                fx = Managers.Pool.Get(fxHumanPath);
            else
                fx = Managers.Pool.Get(fxPath);

            fx.transform.position = transform.position; 
            Managers.Pool.Release(fx, 2f);  

            if(!damageable.CanTakeDamage()) return;
            
            if(damageables.Add(damageable))
            {
                damageable.TakeDamage(damage);
                itemSlot.UseDurability(1); 
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
