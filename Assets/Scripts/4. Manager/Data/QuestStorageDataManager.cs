using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steamworks;




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
