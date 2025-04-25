using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;
using UnityEngine.Networking;

// 예시 테이블 -> 아이템 같은

public class DataManager : IManager
{

    public DataHandler<GameData.Quest> quests {get; private set;}
    public DataHandler<GameData.Data> datas {get; private set;}
    public DataHandler<GameData.QuestTask> questTasks {get; private set;}


    public void Init()
    {
        datas = new DataHandler<GameData.Data>();
        quests = new DataHandler<GameData.Quest>();
        questTasks = new DataHandler<GameData.QuestTask>();

    }

    public void Clear()
    {
        
    }



}
