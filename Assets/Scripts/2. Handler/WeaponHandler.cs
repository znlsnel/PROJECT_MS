    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StatHandler))]
public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform _weaponSocket;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Weapon _weapon;

    private StatHandler _stat;
     
    private void EquipWeapon(Weapon weapon)
    {
        weapon.transform.SetParent(_weaponSocket, false);
        _weapon.UnSetup();
        _weapon = weapon;
        _weapon.Setup(DealDamage);
    } 

    private void Awake()
    {
        _stat = GetComponent<StatHandler>();
        _weapon.Setup(DealDamage);
    }

    // public void Attack(AttackInfoData attackInfoData)
    // {
    //     _weapon.StartAttack(_targetLayer);  
    //     _weapon.EndAttack(attackInfoData.DamageTime);
    // }

    private void DealDamage(GameObject target, IDamagable damagable)
    {
        damagable.TakeDamage(_stat.GetAttackValue());    
    }
}


