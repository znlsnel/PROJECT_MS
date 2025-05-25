using UnityEngine;
using DG.Tweening;

public class DamageIndicatorUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float inTime = 0.2f;
    [SerializeField] private float outTime = 0.2f;

    private static readonly string _clickSound = "Sound/Player/Ouch_01.mp3";

    public void Awake()
    {
        canvasGroup.alpha = 0;
        Managers.onChangePlayer += (player) =>{
            player.onDamaged += OnDamaged; 
        };
    } 

    private void OnDamaged() 
    {
        canvasGroup.DOKill(); 
        canvasGroup.DOFade(1, inTime).SetEase(Ease.OutQuint).SetDelay(0.1f).onComplete += ()=>{
            canvasGroup.DOFade(0, outTime).SetEase(Ease.InCubic);
        };

        Managers.Sound.Play(_clickSound);
    }



}
