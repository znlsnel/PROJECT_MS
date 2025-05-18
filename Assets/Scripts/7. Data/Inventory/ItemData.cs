using System;
using UnityEngine;

public class ItemData
{
    public int Id {get; protected set;}
    public string Name {get; protected set;}
    public string Description {get; protected set;}
    public string PrefabPath {get; protected set;}
    public string DropPrefabPath {get; protected set;}
    public Sprite Icon {get; protected set;}
    public bool CanStack {get; protected set;}
    public int MaxStack {get; protected set;}

    public EItemType ItemType {get; protected set;}
    public EEquipType EquipType {get; protected set;}


    public float Damage {get; protected set;}
    public bool HasDurability {get; protected set;}
    public float MaxDurability {get; protected set;}

    public float Heal {get; protected set;}
    public float RestoreHunger {get; protected set;}
    public float RestoreWater {get; protected set;}
    public float RestoreStamina {get; protected set;}
    public float RestoreTemperature {get; protected set;}
    public float RestoreSanity {get; protected set;}

    public bool SlopeLimit {get; protected set;}
    public float MaxSlopeAngle {get; protected set;}
    public bool OverlapCheck {get; protected set;}

    public ItemData() {}
    public ItemData(GameData.Item item)
    { 
        Setup(item);
    }

    protected virtual void Setup(GameData.Item item)
    {
        Id = item.index;
        Name = item.name;
        Description = item.description;
        PrefabPath = item.prefab;
        DropPrefabPath = item.dropPrefab;
        Managers.Resource.LoadAsync<Texture2D>(item.icon, (texture) =>
        {
            Icon = texture.ToSprite(); 
        });
 
        CanStack = item.canStack;
        MaxStack = item.maxStack;
        ItemType = item.itemType;
        EquipType = item.equipType;  

        Damage = item.damage;
        HasDurability = item.hasDurability;
        MaxDurability = item.durability;

        Heal = item.health;
        RestoreHunger = item.hunger;
        RestoreWater = item.water;
        RestoreStamina = item.stamina;
        RestoreTemperature = item.temperature;
        RestoreSanity = item.sanity;

        SlopeLimit = item.slopeLimit;
        MaxSlopeAngle = item.maxSlopeAngle;
        OverlapCheck = item.overlapCheck;
    }

    protected virtual void Setup(ItemData item)
    {
        Id = item.Id;
        Name = item.Name;
        Description = item.Description;
        PrefabPath = item.PrefabPath;
        Icon = item.Icon;
        CanStack = item.CanStack;
        MaxStack = item.MaxStack;
        ItemType = item.ItemType;
        EquipType = item.EquipType;

        Damage = item.Damage;
        HasDurability = item.HasDurability;
        MaxDurability = item.MaxDurability;

        Heal = item.Heal;
        RestoreHunger = item.RestoreHunger;
        RestoreWater = item.RestoreWater;
        RestoreStamina = item.RestoreStamina;
        RestoreTemperature = item.RestoreTemperature;
        RestoreSanity = item.RestoreSanity;

        SlopeLimit = item.SlopeLimit;
        MaxSlopeAngle = item.MaxSlopeAngle;
        OverlapCheck = item.OverlapCheck;
    }
}
