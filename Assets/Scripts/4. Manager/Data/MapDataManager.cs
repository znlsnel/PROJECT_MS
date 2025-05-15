using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameData;


public class MapData
{
    public int Index {get; private set;}
    public string Name {get; private set;}
    public QuestData MainQuest {get; private set;}
    public QuestData SubQuest {get; private set;}

    public int FieldItemDistribution {get; private set;}
    public int FieldResourceDistribution {get; private set;}

    public List<ItemData> FieldItemList {get; private set;} = new();
    public List<int> FieldItemRatio {get; private set;} = new();
    public List<FieldResourceData> FieldResourceList {get; private set;} = new();
    public List<int> FieldResourceRatio {get; private set;} = new();
    
    public MapData(GameData.Map map)
    {
        Index = map.index;
        Name = map.name;
        MainQuest = Managers.Data.quests.GetByIndex(map.mainQuest);
        SubQuest = Managers.Data.quests.GetByIndex(map.subQuest);

        FieldItemDistribution = map.fieldItemDistribution;
        FieldResourceDistribution = map.fieldResourceDistribution;


        for (int i = 0; i < map.fieldItems.Count; i++)
        {
            FieldItemList.Add(Managers.Data.items.GetByIndex(map.fieldItems[i]));
            FieldItemRatio.Add(map.fieldItemRatio[i]);
        }

        for (int i = 0; i < map.fieldResource.Count; i++)
        {
            FieldResourceList.Add(Managers.Data.fieldResources.GetByIndex(map.fieldResource[i]));
            FieldResourceRatio.Add(map.resourceRatio[i]);
        }
    }
}

public class MapDataManager : DataHandler<MapData>
{
    UGSDataHandler<GameData.Map> dataHandler;

    protected override void Init()
    {
        dataHandler = new UGSDataHandler<GameData.Map>();
        List<GameData.Map> maps = dataHandler.GetAll();
        foreach (GameData.Map map in maps)
        {
            MapData mapData = new MapData(map);
            datas.Add(map.index, mapData);
            dataList.Add(mapData);
        }
    }
}
