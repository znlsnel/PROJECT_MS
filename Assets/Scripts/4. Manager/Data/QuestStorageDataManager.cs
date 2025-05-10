using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steamworks;


public class QuestStorageData
{
    public RequireItem requireItem1;
    public RequireItem requireItem2;
    public RequireItem requireItem3;
    public RequireItem requireItem4;
    public RequireItem requireItem5;

    public List<RequireItem> items {get; private set;} = new List<RequireItem>();

    public QuestStorageData(GameData.QuestStorage questStorage)
    {
        Action<RequireItem, (int, int)> Create = (RequireItem item, (int, int) requireItem) => {
            items.Add(item);
            ItemData itemData = Managers.Data.items.GetByIndex(requireItem.Item1);

            if (itemData == null)
                return;

            item = new RequireItem(itemData, requireItem.Item2);
            items.Add(item);
        };
        
        Create(requireItem1, questStorage.item1);
        Create(requireItem2, questStorage.item2);
        Create(requireItem3, questStorage.item3);
        Create(requireItem4, questStorage.item4);
        Create(requireItem5, questStorage.item5);
    }
}

public class QuestStorageDataManager : DataHandler<QuestStorageData>
{
    private UGSDataHandler<GameData.QuestStorage> questStorageTable;
    protected override void Init()
    {
        if (questStorageTable != null)
            return;

        questStorageTable = new UGSDataHandler<GameData.QuestStorage>();

        List<GameData.QuestStorage> questStorages = questStorageTable.GetAll();
        foreach (GameData.QuestStorage questStorage in questStorages)
        {
            QuestStorageData questStorageData = new QuestStorageData(questStorage);
            datas.Add(questStorage.index, questStorageData);
            dataList.Add(questStorageData);
        }
    }

}
