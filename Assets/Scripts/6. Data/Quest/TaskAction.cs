using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[GoogleSheet.Core.Type.UGS(typeof(ETaskActionType))]
public enum ETaskActionType
{
    PositiveCount, // 3번 점프하기 
    NegativeCount, // 특정 구조물에 다가가기 (현재 거리 120m) => 0m가 되면 성공
    ContinuousCount, // 연속 성공하지 못하면 0으로 초기화 (강화 10회 연속 성공하기)
}

public static class TaskAction
{
    private static Dictionary<ETaskActionType,Func<int, int, int>> actions = new Dictionary<ETaskActionType, Func<int, int, int>>(){
        {ETaskActionType.PositiveCount, (currentCount, successCount) => currentCount + successCount},
        {ETaskActionType.NegativeCount, (currentCount, successCount) => currentCount - successCount},
        {ETaskActionType.ContinuousCount, (currentCount, successCount) => successCount > 0 ? currentCount + successCount : 0},
    }; 

    public static int Run(ETaskActionType actionType, int currentCount, int successCount)
    {
        return actions[actionType](currentCount, successCount);
    }
} 
