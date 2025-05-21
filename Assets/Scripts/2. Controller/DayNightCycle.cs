using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    private float time;
    private float fullDayLength;
    private float startTime = 0.4f;
	private float timeRate;
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
        startTime = timeSystem.startTime;
        fullDayLength = timeSystem.fullDayLength; 
 
        sunIntensity.ClearKeys();
        sunIntensity.AddKey(timeSystem.dayStartTime, 0f, 0f, 0f);
        sunIntensity.AddKey((timeSystem.dayEndTime + timeSystem.dayStartTime) / 2f, 1f, 0f, 0f);
        sunIntensity.AddKey(timeSystem.dayEndTime, 0f, 0f, 0f);


        moonIntensity.ClearKeys();
        moonIntensity.AddKey(0f, 1f, 0f, 0f);
        moonIntensity.AddKey(timeSystem.dayStartTime, 0f, 0f, 0f);
        moonIntensity.AddKey(timeSystem.dayEndTime, 0f, 0f, 0f);
        moonIntensity.AddKey(1f, 1f, 0f, 0f);  

        float multiplier = (timeSystem.dayStartTime + timeSystem.dayEndTime) / 3f;
        IntensityMultiplier.ClearKeys();
        IntensityMultiplier.AddKey(0, 0f, 0f, 0f); 
        IntensityMultiplier.AddKey(multiplier, 1f, 0f, 0f);
        IntensityMultiplier.AddKey(multiplier * 2f, 1f, 0f, 0f);  
        IntensityMultiplier.AddKey(1, 0f, 0f, 0f);

		timeRate = 1.0f / fullDayLength;
		time = startTime;
	}

	private void Update()
	{

        
		time = (time + timeRate * Time.deltaTime) % 1.0f;

		UpdateLighting(sun, sunColor, sunIntensity);
		UpdateLighting(moon, moonColor, moonIntensity);

		RenderSettings.ambientIntensity = IntensityMultiplier.Evaluate(time);
		RenderSettings.reflectionIntensity = IntensityMultiplier.Evaluate(time);
	}

	void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
	{
		float intensity = intensityCurve.Evaluate(time);
		lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
		lightSource.color = gradient.Evaluate(time);
		lightSource.intensity = intensity;

		var go = lightSource.gameObject;
		if (lightSource.intensity == 0 && go.activeInHierarchy)
			go.SetActive(false);

		else if (lightSource.intensity > 0 && !go.activeInHierarchy)
			go.SetActive(true);
	}
}
