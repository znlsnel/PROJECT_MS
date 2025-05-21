using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

 
	[Header("Sun")] 
	[SerializeField] private Light sun;
	[SerializeField] private Gradient sunColor; 
	[SerializeField] private AnimationCurve sunIntensity;

	[Header("Moon")]
	[SerializeField] private Light moon;
	[SerializeField] private Gradient moonColor;
	[SerializeField] private AnimationCurve moonIntensity;

	[Header("Other Lighting")]
	[SerializeField] private AnimationCurve IntensityMultiplier;
	[SerializeField] private AnimationCurve reflectionIntensityMultiplier; 

    private float curTime;
    private float targetTime;
	private float timeScale;

	private Vector3 noon = new Vector3(90, 0, 0);
	private Vector3 endNoon = new Vector3(180, 0, 0); 
	private Vector3 startNoon = new Vector3(0, 0, 0);   


    private TimeSystem timeSystem;

	private void Start()
	{
        timeSystem = Managers.scene.GetComponent<TimeSystem>();

        sunIntensity.ClearKeys();
        sunIntensity.AddKey(0f, 0f, 0f, 0f);
        sunIntensity.AddKey(timeSystem.dayStartTime, 1f, 0f, 0f);
        sunIntensity.AddKey(timeSystem.dayEndTime, 1f, 0f, 0f); 
        sunIntensity.AddKey(1f, 0f, 0f, 0f);   


        moonIntensity.ClearKeys();
        moonIntensity.AddKey(0f, 1f, 0f, 0f);
        moonIntensity.AddKey(timeSystem.dayStartTime, 0f, 0f, 0f);  
        moonIntensity.AddKey(timeSystem.dayEndTime, 0f, 0f, 0f);  
        moonIntensity.AddKey(1f, 1f, 0f, 0f);  


        float multiplier = (timeSystem.dayStartTime + timeSystem.dayEndTime) / 3f;
        IntensityMultiplier.ClearKeys();
        IntensityMultiplier.AddKey(0f, 0f, 0f, 0f);
        IntensityMultiplier.AddKey(timeSystem.dayStartTime, 1f, 0f, 0f);
        IntensityMultiplier.AddKey(timeSystem.dayEndTime, 1f, 0f, 0f); 
        IntensityMultiplier.AddKey(1f, 0f, 0f, 0f);   


		timeScale = timeSystem.TimeScale;
        timeSystem.Time.OnChange += UpdateTime;
	}
 
	private void Update()
	{
        // timeScale초에 한번씩 time에 1f/1440f가 더해짐
      //  curTime = Mathf.MoveTowards(curTime, targetTime, Time.deltaTime / timeScale);      
		curTime += 1f/1440f * (Time.deltaTime/ timeScale);    

		UpdateLighting(sun, sunColor, sunIntensity);
		UpdateLighting(moon, moonColor, moonIntensity);

		RenderSettings.ambientIntensity = IntensityMultiplier.Evaluate(curTime); 
		RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(curTime);
	}
 
    private void UpdateTime(float previous, float current, bool isServer)
    {
		if (Mathf.Abs(curTime - previous) > 1f/1440f)
        {
			Debug.Log("안돼!");  
			curTime = current;   
		}
    }

	void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
	{
		float dayT = (curTime - timeSystem.dayStartTime) / (timeSystem.dayEndTime - timeSystem.dayStartTime);  
		float nightT = 0f;

		// nightT 계산: dayEndTime에서 0, 밤 중간에 1, 다음날 dayStartTime에서 다시 0
		if (curTime >= timeSystem.dayEndTime || curTime <= timeSystem.dayStartTime) 
		{ 
			// 전체 밤 시간의 길이 계산 (자정을 넘어가는 구간 처리)
			float nightDuration = (1f - timeSystem.dayEndTime) + timeSystem.dayStartTime;
			float nightSize = (nightDuration - (1f - timeSystem.dayEndTime)) / nightDuration; 
			float daySize = 1f - nightSize;

			// 현재 밤 시간대에서의 진행도 계산 (0 ~ 1)
			float nightProgress;
			if (curTime >= timeSystem.dayEndTime) 
			{
				// dayEndTime ~ 자정까지
				nightProgress = ((curTime - timeSystem.dayEndTime) / (1f - timeSystem.dayEndTime)) * nightSize; // 0 ~ 0.5 범위
			}
			else 
			{
				// 자정 ~ dayStartTime까지
				nightProgress = (curTime / timeSystem.dayStartTime) * daySize; // 0.5 ~ 1 범위
				nightProgress += nightSize;
			}
			
			// 부드러운 곡선 (sin 곡선 사용)
			nightT = nightProgress;    
		}
 
		if (lightSource == sun)
			lightSource.transform.eulerAngles = Vector3.Lerp(startNoon, endNoon, dayT); 
		else 
			lightSource.transform.eulerAngles = Vector3.Lerp(endNoon, startNoon, nightT);  
		

		float intensity = intensityCurve.Evaluate(curTime);  
		lightSource.color = gradient.Evaluate(curTime);
		lightSource.intensity = intensity;

		var go = lightSource.gameObject;
		if (lightSource.intensity == 0 && go.activeInHierarchy)
			go.SetActive(false);

		else if (lightSource.intensity > 0 && !go.activeInHierarchy)
			go.SetActive(true);
	}
}
