using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;
using UnityEngine.Networking;

// 예시 테이블 -> 아이템 같은

[Serializable]
public class DataManager : IManager
{
    public QuestDataManager quests {get; private set;}
    public ItemDataManager items {get; private set;}
    public QuestTaskDataManager questTasks {get; private set;}
    public CraftingDataManager craftings {get; private set;}
    public QuestStorageDataManager questStorages {get; private set;}
    public SystemDialogDataManager ruleGuides {get; private set;}  
    public MapDataManager maps {get; private set;}
    public FieldResourceDataManager fieldResources {get; private set;}
    [field: SerializeField] public AnimationDataManager animation {get; private set;}

    public void Init()
    {
        questTasks = new QuestTaskDataManager(); 
        quests = new QuestDataManager();
        
        items = new ItemDataManager(); 
        craftings = new CraftingDataManager();
        questStorages = new QuestStorageDataManager();
        fieldResources = new FieldResourceDataManager();

        ruleGuides = new SystemDialogDataManager();
        maps = new MapDataManager();

        animation = new AnimationDataManager();
        animation.Init();
    }

    public void Clear()
    {
        
    }



}
