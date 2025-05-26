using System;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    Black,
    Gray,
    White,
    Red,
    Orange,
    Yellow,
    LightGreen,
    Green,
    Turquoise,
    Blue,
    Indigo,
    Purple,
    Pink,
    Brown,
    Gold,
    Mint,
}

[CreateAssetMenu(fileName = "ColorSO", menuName = "Data/ColorSO")]
[System.Serializable]
public class ColorSO : ScriptableObject
{
    [Header("색상 설정")]
    [SerializeField] private Color[] colors = new Color[16];
    
    private static Dictionary<ColorType, Color> ColorMapping = new Dictionary<ColorType, Color>();

    public void Init()
    {
        InitializeColorMappings();
    }

    /// <summary>
    /// 색상 배열을 기반으로 ColorMapping을 초기화합니다.
    /// </summary>
    private void InitializeColorMappings()
    {
        ColorMapping.Clear();
        ColorType[] colorTypes = (ColorType[])Enum.GetValues(typeof(ColorType));
        
        for (int i = 0; i < colorTypes.Length && i < colors.Length; i++)
        {
            ColorMapping[colorTypes[i]] = colors[i];
        }
    }

    /// <summary>
    /// 에디터에서 색상이 변경되었을 때 호출됩니다.
    /// </summary>
    public void OnColorsChanged()
    {
        InitializeColorMappings();
    }

    /// <summary>
    /// 기본 색상으로 초기화합니다.
    /// </summary>
    public void ResetToDefaultColors()
    {
        colors = new Color[]
        {
            "#000000".ConvertColor(),
            "#808080".ConvertColor(),
            "#FFFFFF".ConvertColor(),
            "#E41A1C".ConvertColor(),
            "#FF7F00".ConvertColor(),
            "#FFFF33".ConvertColor(),
            "#B2DF8A".ConvertColor(),
            "#4DAF4A".ConvertColor(),
            "#00CED1".ConvertColor(),
            "#377EB8".ConvertColor(),
            "#000080".ConvertColor(),
            "#984EA3".ConvertColor(),
            "#F781BF".ConvertColor(),
            "#A65628".ConvertColor(),
            "#FFD700".ConvertColor(),
            "#66C2A5".ConvertColor(),
        };
        OnColorsChanged();
    }

    #region Static 색상 변환 메서드들

    /// <summary>
    /// ColorType을 Unity Color로 변환합니다.
    /// </summary>
    public static Color GetColor(ColorType colorType)
    {
        return ColorMapping.TryGetValue(colorType, out Color color) ? color : Color.white;
    }

    /// <summary>
    /// 모든 색상 매핑을 반환합니다.
    /// </summary>
    public static Dictionary<ColorType, Color> GetAllColorMappings()
    {
        return new Dictionary<ColorType, Color>(ColorMapping);
    }

    #endregion

    #region 에디터 전용 프로퍼티

    /// <summary>
    /// 에디터에서 색상 배열에 접근하기 위한 프로퍼티
    /// </summary>
    public Color[] Colors
    {
        get => colors;
        set
        {
            colors = value;
            OnColorsChanged();
        }
    }

    /// <summary>
    /// 특정 ColorType의 색상을 가져오거나 설정합니다.
    /// </summary>
    public Color this[ColorType colorType]
    {
        get
        {
            int index = (int)colorType;
            return index < colors.Length ? colors[index] : Color.white;
        }
        set
        {
            int index = (int)colorType;
            if (index < colors.Length)
            {
                colors[index] = value;
                OnColorsChanged();
            }
        }
    }

    #endregion
}
