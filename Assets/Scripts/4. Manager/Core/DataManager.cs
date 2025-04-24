using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;
using UnityEngine.Networking;

// 예시 테이블 -> 아이템 같은

public class DataManager : Manager
{

   // public DataHandler<CharacterData.CharacterData> Characters {get; private set;}

     
    public event Action onDataLoaded;
    private bool isDataLoaded = false;

    // 테이블 하나로 통합 -> 너무 많은 분리는 관리 어려움
    protected override void Init()
    {

        isDataLoaded = true;
        onDataLoaded?.Invoke();
        onDataLoaded = null;

    }

    public void SubscribeToDataLoaded(Action callback) 
    {
        if (isDataLoaded)
            callback?.Invoke();
        else
            onDataLoaded += callback;
    }

    public override void Clear()
    {
        
    }



}
