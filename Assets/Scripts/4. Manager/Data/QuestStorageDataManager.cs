using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steamworks;


public class QuestStorageData
{
    public List<RequireItem> items {get; private set;} = new List<RequireItem>();

    public QuestStorageData(GameData.QuestStorage questStorage)
    {
        Action<(int, int)> Create = ((int, int) requireItem) => {
            ItemData itemData = Managers.Data.items.GetByIndex(requireItem.Item1);

            if (itemData == null)
                return;

            items.Add(new RequireItem(itemData, requireItem.Item2));
        };
        
        Create(questStorage.item1);
        Create(questStorage.item2);
        Create(questStorage.item3);
        Create(questStorage.item4);
        Create(questStorage.item5);
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
