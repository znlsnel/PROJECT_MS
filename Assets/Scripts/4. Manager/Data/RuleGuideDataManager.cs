using System.Collections.Generic;
using UnityEngine;

public class SystemDialogDataManager 
{
    UGSDataHandler<GameData.SystemDialog> dataHandler;

    public SystemDialogDataManager()
    {
        dataHandler = new UGSDataHandler<GameData.SystemDialog>();
    }

    public List<string> GetScript(bool isMafia = false)
    {
        int jobId = isMafia ? 2 : 1;
        GameData.SystemDialog systemDialog = dataHandler.GetByCondition((systemDialog) => systemDialog.jobId == jobId);
        if (systemDialog == null)
            return null;

        return systemDialog.texts;
    }
}