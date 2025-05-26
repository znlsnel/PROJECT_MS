using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnalyticsManager : IManager
{
    private bool isInitialized = false;
    
    public async void Init()
    {
        try
        {
            if (!isInitialized)
            {
                await UnityServices.InitializeAsync();
                AnalyticsService.Instance.StartDataCollection();
                isInitialized = true;
                Debug.Log("Analytics Manager 초기화 완료");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Analytics Manager 초기화 실패: {e.Message}");
        }
    }
    
    public void Clear()
    {
        // 필요시 정리 작업
    }
    
    // 플레이어 세션 시작
    public void SurvivalStart()
    {
        if (!isInitialized) return;
        

        AnalyticsService.Instance.RecordEvent("SurvivalStart");
    }
    
    // 게임 종료 이벤트
    public void SurvivalEnding()
    {
        if (!isInitialized) return;
        

        AnalyticsService.Instance.RecordEvent("SurvivalEnding");
    }

    // 게임 강제 종료 이벤트
    public void SurvivalQuit()
    {
        if (!isInitialized) return;

        var timeSystem = Managers.scene.GetComponent<TimeSystem>();


        CustomEvent parameters = new CustomEvent("SurvivalQuit")
        {
            ["Day"] = timeSystem.CurrentDay,
            ["Hour"] = timeSystem.CurrentHour,
            ["Minute"] = timeSystem.CurrentMinute, 
        };

        AnalyticsService.Instance.RecordEvent(parameters);
    }
 
    // 채팅 사용량
    public void ChatUsage()
    {
        if (!isInitialized) return;

        AnalyticsService.Instance.RecordEvent("ChatUsage");
    }
    

    // 제작 사용량
    public void CraftingUsage()
    {
        if (!isInitialized) return;

        AnalyticsService.Instance.RecordEvent("CraftingUsage");
    }

    // 마피아 vs 생존자 승률
    public void MafiaWinRate(bool isMafiaWin)
    {
        if (!isInitialized) return;

        var parameters = new CustomEvent("MafiaWinRate")
        {
            ["MafiaWin"] = isMafiaWin
        };

        AnalyticsService.Instance.RecordEvent(parameters);
    }
    
    // 생존률
    public void SurvivalRate(bool isSurvival)
    {
        if (!isInitialized) return;

        var parameters = new CustomEvent("SurvivalRate") 
        {
            ["Survival"] = isSurvival 
        };

        AnalyticsService.Instance.RecordEvent(parameters);
    }
}  