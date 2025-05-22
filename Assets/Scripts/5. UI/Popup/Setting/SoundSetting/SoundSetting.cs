using UnityEngine;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private SlideValueUI masterValue;
    [SerializeField] private SlideValueUI effectValue;
    [SerializeField] private SlideValueUI bgmValue;

    private void Awake()
    {
        masterValue.onValueChanged += SetMasterVolume;
        effectValue.onValueChanged += SetEffectVolume;
        bgmValue.onValueChanged += SetBgmVolume;
    }

    private void SetMasterVolume(float value)
    {
       
    }

    private void SetEffectVolume(float value)   
    {

    }

    private void SetBgmVolume(float value)
    {

    }
    
}
