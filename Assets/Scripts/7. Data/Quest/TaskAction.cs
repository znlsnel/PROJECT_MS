using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




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
