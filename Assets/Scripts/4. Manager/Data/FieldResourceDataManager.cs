using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;


public class FieldResourceData
{
    public int index {get; private set;}
    public List<ItemData> dropItemList {get; private set;} = new();
    public string PrefabPath {get; private set;}
    public bool canRotate_Roll {get; private set;}
    public bool canRotate_Yaw {get; private set;}
    public bool canRotate_Pitch {get; private set;}

    public (int, int) sizeRate {get; private set;}

    public FieldResourceData(GameData.FieldResource fieldResource)
    {
        index = fieldResource.index;
        PrefabPath = fieldResource.prefab;
        canRotate_Roll = fieldResource.canRotate_Roll;
        canRotate_Yaw = fieldResource.canRotate_Yaw;
        canRotate_Pitch = fieldResource.canRotate_Pitch;
        sizeRate = fieldResource.sizeRate;
    }
}

public class FieldResourceDataManager : DataHandler<FieldResourceData>
{
    UGSDataHandler<GameData.FieldResource> dataHandler;

    protected override void Init()
    {
        dataHandler = new UGSDataHandler<GameData.FieldResource>();
        List<GameData.FieldResource> fieldResources = dataHandler.GetAll();
        foreach (GameData.FieldResource fieldResource in fieldResources)
        {
            FieldResourceData fieldResourceData = new FieldResourceData(fieldResource);
            datas.Add(fieldResource.index, fieldResourceData);
            dataList.Add(fieldResourceData);
        }
    }
}
