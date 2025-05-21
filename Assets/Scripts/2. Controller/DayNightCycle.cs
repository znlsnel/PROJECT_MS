using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    private float time;
    private float fullDayLength;
	private float timeScale;
	private Vector3 noon = new Vector3(90, 0, 0); // Vector 90 0 0

	[Header("Sun")]
	public Light sun;
	public Gradient sunColor;
	public AnimationCurve sunIntensity;

	[Header("Moon")]
	public Light moon;
	public Gradient moonColor;
	public AnimationCurve moonIntensity;

	[Header("Other Lighting")]
	public AnimationCurve IntensityMultiplier;
	public AnimationCurve reflectionIntensityMultiplier;


    private TimeSystem timeSystem;

	private void Start()
	{
        timeSystem = Managers.scene.GetComponent<TimeSystem>();
        fullDayLength = timeSystem.fullDayLength; 
 
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
        time += 1f/1440f * (Time.deltaTime/ timeScale);   
        Debug.Log(time); 

		UpdateLighting(sun, sunColor, sunIntensity);
		UpdateLighting(moon, moonColor, moonIntensity);

		RenderSettings.ambientIntensity = IntensityMultiplier.Evaluate(time); 
		RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
	}
 
    private void UpdateTime(float previous, float current, bool isServer)
    {
        time = current;
    }

	void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
	{
		float intensity = intensityCurve.Evaluate(time); 
		lightSource.transform.eulerAngles = (time - (lightSource == sun ? timeSystem.dayStartTime : timeSystem.dayEndTime)) * noon * 4f; 
		lightSource.color = gradient.Evaluate(time);
		lightSource.intensity = intensity;

		var go = lightSource.gameObject;
		if (lightSource.intensity == 0 && go.activeInHierarchy)
			go.SetActive(false);

		else if (lightSource.intensity > 0 && !go.activeInHierarchy)
			go.SetActive(true);
	}
}
