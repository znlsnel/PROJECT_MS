using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class QuestStorageData
{

}

public class QuestStorageDataManager : DataHandler<QuestStorageData>
{
    private UGSDataHandler<GameData.QuestStorage> questStorageTable;
    protected override void Init()
    {

    }

}
