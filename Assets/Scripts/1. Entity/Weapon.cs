using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Collider _monsterSensor;
    private LayerMask _targetLayer;

    private HashSet<GameObject> _attackTargets = new HashSet<GameObject>();

    // 데미지 처리를 받을 몬스터 정보 불러오기
    public event Action<GameObject, IDamageable> onAttack; 

    public void Setup(Action<GameObject, IDamageable> attackCallback)
    {
        onAttack = null;
        onAttack += attackCallback; 
    }
    
    public void UnSetup()
    {
        onAttack = null;
    }

    public void StartAttack(LayerMask targetLayer)
    {
        Debug.Log("Start Attack ! ");
        _targetLayer = targetLayer;
        _monsterSensor.enabled = true;
    }

    public void EndAttack(float delay)
    {
        Debug.Log("End Attack ! "); 
        Invoke(nameof(EndAttack), delay); 
    }

    public void EndAttack()
    {
        _monsterSensor.enabled = false; 
        _attackTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_attackTargets.Contains(other.gameObject))
            return;

        IDamageable damagable = other.gameObject.GetComponent<IDamageable>();
        if (damagable != null)
        {
            _attackTargets.Add(other.gameObject);
            onAttack?.Invoke(other.gameObject, damagable); 
        }
    }  
}