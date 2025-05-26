
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class Extension
{
    public static uint ConvertToUInt(this float value)
    {
        if (value < 0)
            return (uint)(Mathf.Abs(value) * 1000) | 0x80000000;
        return (uint)(value * 1000);
    }

    public static float ConvertToFloat(this uint value)
    {
        if ((value & 0x80000000) != 0)
            return -((value & 0x7FFFFFFF) / 1000f);
        return value / 1000f; 
    } 


    public static float GetXZMagnitude(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z).magnitude; 
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
    }
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        return Util.FindChild<T>(go, name, recursive); 
    }

	public static Vector3 RandomUnitVectorInCone(this Vector3 direction, float coneHalfAngleDegrees)
	{
		// 방향 벡터 정규화
        Vector3 dirNormalized = direction.normalized;
        
        // 각도를 라디안으로 변환
        float coneHalfAngleRad = coneHalfAngleDegrees * Mathf.Deg2Rad;
        
        // 랜덤 각도 생성 (0에서 coneHalfAngleRad 사이)
        float angle = Random.Range(0f, coneHalfAngleRad);
        
        // 랜덤 방향 각도 생성 (0에서 2π 사이)
        float rotationAngle = Random.Range(0f, 2f * Mathf.PI);
        
        // 구면 좌표계에서 데카르트 좌표계로 변환
        float sinAngle = Mathf.Sin(angle);
        float x = sinAngle * Mathf.Cos(rotationAngle);
        float y = sinAngle * Mathf.Sin(rotationAngle);
        float z = Mathf.Cos(angle);
        
        // 생성된 벡터 (Z 축을 중심으로 한 콘)
        Vector3 randomVector = new Vector3(x, y, z);
        
        // Z 축을 원하는 방향으로 회전시키기 위한 회전 계산
        if (dirNormalized != Vector3.forward)
        {
            // Z 축에서 원하는 방향으로의 회전을 계산
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, dirNormalized);
            randomVector = rotation * randomVector;
        }
        
        return randomVector;

	}

    public static Sprite ToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); 
    }

    public static Color ConvertColor(this string hex)
    {
        // # 제거
        hex = hex.Replace("#", "");
        
        // 16진수 문자열을 정수로 변환
        int r = Convert.ToInt32(hex.Substring(0, 2), 16);
        int g = Convert.ToInt32(hex.Substring(2, 2), 16);
        int b = Convert.ToInt32(hex.Substring(4, 2), 16);
        
        // 0-1 사이 값으로 변환
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public static T Back<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static Dictionary<int, T> ToDictionary<T>(this List<T> list)
    {
        Dictionary<int, T> dictionary = new Dictionary<int, T>();
        for (int i = 0; i < list.Count; i++)
        {
            dictionary.Add(i, list[i]);
        }
        return dictionary;
    }


    public static void AddKey(this AnimationCurve curve, float time, float value, float inTangent, float outTangent)
    {
        curve.AddKey(new Keyframe(time, value, inTangent, outTangent)); 
    }

    public static void SetHeight(this RectTransform rectTransform, float height)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
}
