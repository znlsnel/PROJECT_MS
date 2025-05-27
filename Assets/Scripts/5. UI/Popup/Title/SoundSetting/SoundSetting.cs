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
 

    private void Start()
    {
        masterValue.SetValue(Managers.Sound.MasterVolume); 
        effectValue.SetValue(Managers.Sound.EffectVolume); 
        bgmValue.SetValue(Managers.Sound.BgmVolume);
    }

    private void SetMasterVolume(float value)
    {
      Managers.Sound.SetMasterVolume(value);
    }

    private void SetEffectVolume(float value)
    {
       Managers.Sound.SetVolume(ESound.Effect, value);
    }

    private void SetBgmVolume(float value)
    {
       Managers.Sound.SetVolume(ESound.Bgm, value);
    }
}
