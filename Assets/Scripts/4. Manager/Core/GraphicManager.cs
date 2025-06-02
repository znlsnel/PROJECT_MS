using UnityEngine;

[System.Serializable]
public class GraphicManager : IManager
{
    [field:SerializeField] public EGraphic quality {get; private set;} = EGraphic.Low;
    [field:SerializeField] public int width {get; private set;} = 1920;
    [field:SerializeField] public int height {get; private set;} = 1080;
    [field:SerializeField] public bool isFullScreen {get; private set;} = true;

    public void Init()
    {
        quality = (EGraphic)QualitySettings.GetQualityLevel();
        width = Screen.currentResolution.width;
        height = Screen.currentResolution.height;
        isFullScreen = Screen.fullScreen;
    }

    public void Clear()
    {
        
    }

    public void SetQuality(EGraphic quality)
    {
        this.quality = quality;
        QualitySettings.SetQualityLevel((int)quality);
    }

    public void SetResolution(int width, int height)
    {
        this.width = width;
        this.height = height;
        Screen.SetResolution(width, height, isFullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
        Screen.SetResolution(width, height, isFullScreen);
    }
}

public enum EGraphic
{
    Low,
    Normal,
    High,
    VeryHigh,
}
