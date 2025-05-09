using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class QuestTaskDataManager : DataHandler<TaskData>
{
    private UGSDataHandler<GameData.QuestTask> questTaskTable;

    protected override void Init()
    {
        if (questTaskTable != null)
            return;

        questTaskTable = new UGSDataHandler<GameData.QuestTask>();

        List<GameData.QuestTask> questTasks = questTaskTable.GetAll();
        foreach (GameData.QuestTask questTask in questTasks)
        {
            TaskData questTaskData = new TaskData(questTask);
            datas.Add(questTask.index, questTaskData); 
            dataList.Add(questTaskData);
        } 
    }
}
