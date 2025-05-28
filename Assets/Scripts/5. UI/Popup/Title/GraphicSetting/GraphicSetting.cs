using System;
using TMPro;
using UnityEngine;

public class GraphicSetting : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _fullScreenDropdown;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    public void Start()
    {
        _fullScreenDropdown.onValueChanged.AddListener(OnFullScreenChanged);
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        _qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

        _fullScreenDropdown.value = Managers.Graphic.isFullScreen ? 0 : 1;
        _resolutionDropdown.value = Managers.Graphic.width == 1280 ? 0 : Managers.Graphic.width == 1920 ? 1 : 2;
        _qualityDropdown.value = (int)Managers.Graphic.quality;
    }

    private void OnFullScreenChanged(int arg0)
    {
        Managers.Graphic.SetFullScreen(arg0 == 0);
    }

    private void OnResolutionChanged(int arg0)
    {
        switch(arg0)
        {
            case 0:
                Managers.Graphic.SetResolution(1280, 720);
                break;
            case 1:
                Managers.Graphic.SetResolution(1920, 1080);
                break;
            case 2:
                Managers.Graphic.SetResolution(2560, 1440);
                break;
        }
    }
    
    private void OnQualityChanged(int arg0)
    {
        Managers.Graphic.SetQuality((EGraphic)arg0);
    }
}
