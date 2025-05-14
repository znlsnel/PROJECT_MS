using System.Collections.Generic;
using UnityEngine;

public class RuleGuideDataManager 
{
    UGSDataHandler<GameData.RuleGuide> dataHandler;

    public RuleGuideDataManager()
    {
        dataHandler = new UGSDataHandler<GameData.RuleGuide>();
    }

    public List<string> GetScript(bool isMafia = false)
    {
        int jobId = isMafia ? 2 : 1;
        GameData.RuleGuide ruleGuide = dataHandler.GetByCondition((ruleGuide) => ruleGuide.jobId == jobId);
        if (ruleGuide == null)
            return null;

        return ruleGuide.texts;
    }
}