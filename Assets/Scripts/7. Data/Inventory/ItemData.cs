using System;
using UnityEngine;

public class ItemData
{
    public int Id {get; protected set;}
    public string Name {get; protected set;}
    public string Description {get; protected set;}
    public string PrefabPath {get; protected set;}
    public Sprite Icon {get; protected set;}
    public bool CanStack {get; protected set;}
    public int MaxStack {get; protected set;}

    public EItemType ItemType {get; protected set;}
    public EEquipType EquipType {get; protected set;}


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
        Managers.Resource.LoadAsync<Texture2D>(item.icon, (texture) =>
        {
            Icon = texture.ToSprite(); 
        });
 
        CanStack = item.canStack;
        MaxStack = item.maxStack;
        ItemType = item.itemType;
        EquipType = item.equipType;  
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

    }
}
