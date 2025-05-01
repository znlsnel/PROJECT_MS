using System;
using UnityEngine;

public class ItemData
{
    public int Id {get; private set;}
    public string Name {get; private set;}
    public string Description {get; private set;}
    public string PrefabPath {get; private set;}
    public Sprite Icon {get; private set;}
    public bool CanStack {get; private set;}
    public int MaxStack {get; private set;}

    public EItemType ItemType {get; private set;}
    public EEquipType EquipType {get; private set;} 

    public ItemData(GameData.Item item) 
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
}
