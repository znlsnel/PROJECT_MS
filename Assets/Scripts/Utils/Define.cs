#region Enums
using UnityEngine;

public enum ESound
{
    Bgm,
    Effect,
    MaxCount,   
}


[GoogleSheet.Core.Type.UGS(typeof(EStatType))]
public enum EStatType 
{
    Health,
    Speed,
}

[GoogleSheet.Core.Type.UGS(typeof(EItemType))]
public enum EItemType 
{
    None,
    Weapon,
    Consumable,
    Resource,
    Equippable,
    Placeable,
}

[GoogleSheet.Core.Type.UGS(typeof(EEquipType))]
public enum EEquipType 
{
    None,
    Head,
    Shirt,
    Pants,
    Shoes,
}

[GoogleSheet.Core.Type.UGS(typeof(ETaskCategory))]
public enum ETaskCategory
{
    Kill,
    Talk,
    UseItem,
    Interact,
    Pickup, 
    Create,
    FillQuestStorage,
}


[GoogleSheet.Core.Type.UGS(typeof(ETaskActionType))]
public enum ETaskActionType
{
    PositiveCount, // 3번 점프하기 
    NegativeCount, // 특정 구조물에 다가가기 (현재 거리 120m) => 0m가 되면 성공
    ContinuousCount, // 연속 성공하지 못하면 0으로 초기화 (강화 10회 연속 성공하기)
}

public enum ETaskState
{
    Inactive,
    Running,
    Complete
}

#endregion

#region Interface
public interface IInteractable
{
    GameObject Interact(GameObject gameObject);
}

public interface IDamageable
{
    void TakeDamage(float damage, GameObject attacker); 
}

public interface IManager
{
    void Init();
    void Clear();
}
#endregion